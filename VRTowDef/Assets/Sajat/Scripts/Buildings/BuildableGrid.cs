using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableGrid : MonoBehaviour
{
    public bool hasBuilding;
    public Building currentBuilding;

    

    private void OnMouseEnter()
    {
        //Debug.Log("hello");
    }

    private void OnMouseExit()
    {
        //Debug.Log("viszl�t!");
    }

    private void OnMouseDown()
    {
        BuildManager.instance.Build(this);
    }

}
