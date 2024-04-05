using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Building),true)]
public class BuildingEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(20);
        Building targetBuilding = target.GetComponent<Building>();

        if (targetBuilding.UpgradeLvl == 0 || !targetBuilding.UpgradeType)
        {
            if (GUILayout.Button("Upgrade")) //Elsõ ág fejlesztése.
            {
                if (!Application.isPlaying) Debug.LogWarning("Nem megy a játék.");
                else
                {
                    targetBuilding.UpgradeType = false;
                    targetBuilding.Upgrade(false);
                }
            }
        }

        if (targetBuilding.UpgradeLvl == 0 || targetBuilding.UpgradeType)
        {
            if (GUILayout.Button("Secondary Upgrade")) //Második ág fejlesztése.
            {
                if (!Application.isPlaying) Debug.LogWarning("Nem megy a játék.");
                else
                {
                    targetBuilding.UpgradeType = true;
                    targetBuilding.Upgrade(true);
                }
            }
        }
    }
}
