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
        //Ha l�trej�n az �p�let, be kell �ket "jelenteni" a hivatalos SteamVR k�dba.
        teleportPoints = GetComponentsInChildren<TeleportMarkerBase>();
        foreach (var teleportPoint in teleportPoints)
        {
            Debug.Log("L�trehoztam �j teleportpontot!");
            Teleport.instance.NewTeleportMarker(teleportPoint);
        }
    }

    public override void Sell()
    {
        //Ha el kell adni az �p�letet, t�r�lj�k a SteamVR-os t�mbb�l a teleportpontokat.
        foreach (var teleportPoint in teleportPoints)
        {
            Teleport.instance.DeletedTeleportMarker(teleportPoint);
        }
        base.Sell();

    }
}
