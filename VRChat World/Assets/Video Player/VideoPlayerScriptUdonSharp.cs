
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDK3.Components.Video;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;

public class VideoPlayerScriptUdonSharp : UdonSharpBehaviour
{

    private BaseVRCVideoPlayer videoPlayer;
    private int loadedVideoNumber;

    [UdonSynced] private VRCUrl syncedURL;
    [UdonSynced] private int currentVideoNumber;
    [UdonSynced] private float videoTime;
    [UdonSynced] private bool ownerPlaying = true;

    public AudioSource speakers;
    public Slider sliderVolume;
    public VRCUrlInputField URLField;
    public GameObject buttonPlay;
    public GameObject buttonPause;

    private float _nextTimeSync;
    private const float TimeSyncInterval = 0.25f;

    void Start()
    {
        videoPlayer = (BaseVRCVideoPlayer)GetComponent(typeof(BaseVRCVideoPlayer));
        loadedVideoNumber = currentVideoNumber;
    }

    public override void OnDeserialization()
    {
        if (loadedVideoNumber != currentVideoNumber)
        {
            loadedVideoNumber = currentVideoNumber;

            if (!VRCUrl.IsNullOrEmpty(syncedURL))
            {
                LoadURL();
            }
        }

        if (!Networking.IsOwner(gameObject))
        {
            if (ownerPlaying != videoPlayer.IsPlaying)
            {
                if (ownerPlaying)
                {
                    videoPlayer.Play();
                    buttonPause.SetActive(true);
                    buttonPlay.SetActive(false);
                }
                else
                {
                    videoPlayer.Pause();
                    buttonPause.SetActive(false);
                    buttonPlay.SetActive(true);
                }
            }
        }
    }

    public void Update()
    {
        if (!videoPlayer) return;

        if (Networking.IsOwner(gameObject) && videoPlayer.IsPlaying)
        {
            if (Time.time >= _nextTimeSync)
            {
                videoTime = videoPlayer.GetTime();
                RequestSerialization();
                _nextTimeSync = Time.time + TimeSyncInterval;
            }    
        }
        else if (!Networking.IsOwner(gameObject) && videoPlayer.IsPlaying)
        {
            if (Mathf.Abs(videoTime - videoPlayer.GetTime()) > 2f)
            {
                videoPlayer.SetTime(videoTime);
            }
        }
    }

    public void PlayPause()
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        if (videoPlayer.IsPlaying)
        {
            videoPlayer.Pause();
            buttonPause.SetActive(false);
            buttonPlay.SetActive(true);
            ownerPlaying = false;
        }
        else
        {
            videoPlayer.Play();
            buttonPause.SetActive(true);
            buttonPlay.SetActive(false);
            ownerPlaying = true;
        }

        RequestSerialization();
    }

    public void SetVolume()
    {
        speakers.volume = sliderVolume.value;
    }

    public void SetURL()
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        var url = URLField.GetUrl();
        if (VRCUrl.IsNullOrEmpty(url)) return;

        syncedURL = url;
        currentVideoNumber += 1;

        videoTime = 0f;
        ownerPlaying = true;

        loadedVideoNumber = currentVideoNumber;

        RequestSerialization();
        LoadURL();
    }

    public void LoadURL()
    {
        videoPlayer.Stop();
        videoPlayer.LoadURL(syncedURL);
    }

    public override void OnVideoReady()
    {
        if (ownerPlaying)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.Pause();
        }

        if (Networking.IsOwner(gameObject))
        {
            buttonPause.SetActive(ownerPlaying);
            buttonPlay.SetActive(!ownerPlaying);
            RequestSerialization();
        }
    }

    public override void OnVideoError(VideoError videoError)
    {
        switch (videoError)
        {
            case VideoError.RateLimited:
            case VideoError.PlayerError:
                loadedVideoNumber = Mathf.Max(loadedVideoNumber - 1, 0);
                break;
        }
    }
}
