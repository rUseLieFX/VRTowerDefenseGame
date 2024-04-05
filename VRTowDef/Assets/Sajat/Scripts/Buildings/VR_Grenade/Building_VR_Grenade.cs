using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Relative egyszer� ez a torony, mivel a f� gimmicket a VR adja, ezzel csak respawnoltatni kell a gr�n�tokat.
public class Building_VR_Grenade : VRBuilding
{
    [SerializeField] VR_ItemSpawner[] spawners;

    private void Start()
    {
        InitTPs();
        spawners = GetComponentsInChildren<VR_ItemSpawner>();
    }


}
