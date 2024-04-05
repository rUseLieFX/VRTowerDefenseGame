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
            if (GUILayout.Button("Upgrade")) //Els� �g fejleszt�se.
            {
                if (!Application.isPlaying) Debug.LogWarning("Nem megy a j�t�k.");
                else
                {
                    targetBuilding.UpgradeType = false;
                    targetBuilding.Upgrade(false);
                }
            }
        }

        if (targetBuilding.UpgradeLvl == 0 || targetBuilding.UpgradeType)
        {
            if (GUILayout.Button("Secondary Upgrade")) //M�sodik �g fejleszt�se.
            {
                if (!Application.isPlaying) Debug.LogWarning("Nem megy a j�t�k.");
                else
                {
                    targetBuilding.UpgradeType = true;
                    targetBuilding.Upgrade(true);
                }
            }
        }
    }
}
