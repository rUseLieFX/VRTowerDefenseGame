using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

//Ez csak debug kód, ezzel lehet a VR Control layout-okat változtatni mindentõl függetlenül.
public class VR_ActionSet_Setter : MonoBehaviour
{
    public SteamVR_ActionSet setToActivate;
    public SteamVR_Input_Sources inputSource;
    public int prio;

    public void ActivateSet(bool activate = true)
    {
        if (activate) setToActivate.Activate(inputSource, prio);
        else setToActivate.Deactivate();
    }
}
[CustomEditor(typeof(VR_ActionSet_Setter), true)]
public class VR_ActionSet_Setter_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VR_ActionSet_Setter vras = (VR_ActionSet_Setter)target;
        if (GUILayout.Button("Aktiválás"))
        {
            vras.ActivateSet();
        }

        if (GUILayout.Button("Deaktiválás"))
        {
            vras.ActivateSet(false);
        }
    }
}
