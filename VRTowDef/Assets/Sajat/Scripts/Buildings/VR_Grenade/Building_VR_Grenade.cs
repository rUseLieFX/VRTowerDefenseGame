using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Relative egyszerû ez a torony, mivel a fõ gimmicket a VR adja, ezzel csak respawnoltatni kell a gránátokat.
public class Building_VR_Grenade : VRBuilding
{
    [SerializeField] VR_ItemSpawner[] spawners;

    private void Start()
    {
        InitTPs();
        spawners = GetComponentsInChildren<VR_ItemSpawner>();
    }


}
