using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VRBuilding : Building

{
    private TeleportMarkerBase[] teleportPoints;

    void Start()
    {
        InitTPs();
    }

    protected virtual void InitTPs() 
    {
        //Ha létrejön az épület, be kell õket "jelenteni" a hivatalos SteamVR kódba.
        teleportPoints = GetComponentsInChildren<TeleportMarkerBase>();
        foreach (var teleportPoint in teleportPoints)
        {
            Debug.Log("Létrehoztam új teleportpontot!");
            Teleport.instance.NewTeleportMarker(teleportPoint);
        }
    }

    public override void Sell()
    {
        //Ha el kell adni az épületet, töröljük a SteamVR-os tömbbõl a teleportpontokat.
        foreach (var teleportPoint in teleportPoints)
        {
            Teleport.instance.DeletedTeleportMarker(teleportPoint);
        }
        base.Sell();

    }
}
