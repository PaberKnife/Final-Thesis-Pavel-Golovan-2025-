
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TeleportScriptUdonSharp : UdonSharpBehaviour
{

    public Transform TargetLocation;

    public override void Interact()
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        if (localPlayer == null || TargetLocation == null) return;

        localPlayer.TeleportTo(TargetLocation.position, TargetLocation.rotation);
    }
}
