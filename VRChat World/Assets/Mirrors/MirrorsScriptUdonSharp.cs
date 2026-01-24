
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MirrorsScriptUdonSharp : UdonSharpBehaviour
{

    public GameObject[] mirrors;

    public void MirrorsTurnOff()
    {
        for(int i = 0; i < mirrors.Length; i++)
        {
            mirrors[i].SetActive(false);
        }
    }

    public void MirrorsTurnOnLow()
    {
        for (int i = 0; i < mirrors.Length; i++)
        {
            if (i == 0)
            {
                mirrors[i].SetActive(true);
            }
            else
            {
                mirrors[i].SetActive(false);
            }
        }
    }

    public void MirrorsTurnOnMedium()
    {
        for (int i = 0; i < mirrors.Length; i++)
        {
            if (i == 1)
            {
                mirrors[i].SetActive(true);
            }
            else
            {
                mirrors[i].SetActive(false);
            }
        }
    }

    public void MirrorsTurnOnHigh()
    {
        for (int i = 0; i < mirrors.Length; i++)
        {
            if (i == 2)
            {
                mirrors[i].SetActive(true);
            }
            else
            {
                mirrors[i].SetActive(false);
            }
        }
    }
}
