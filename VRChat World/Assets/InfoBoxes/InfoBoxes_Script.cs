
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class InfoBoxes_Script : UdonSharpBehaviour
{

    public GameObject affectedObject;

    public override void Interact()
    {
        if (affectedObject.activeSelf == true)
        {
            affectedObject.SetActive(false);
        }
        else
        {
            affectedObject.SetActive(true);
        }
    }
}
