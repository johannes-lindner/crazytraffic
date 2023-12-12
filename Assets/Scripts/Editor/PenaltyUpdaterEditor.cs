using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Pathfinding;


[CustomEditor(typeof(PenaltyUpdater))]
public class PenaltyUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PenaltyUpdater pu = (PenaltyUpdater) target; // target is the current object the PenaltyUpdater Script is attached to


        if (GUILayout.Button("Update Search Points"))
        {
            pu.UpdateSearchPoints();
        }
        if (GUILayout.Button("Update Penalties"))
        {
            pu.UpdatePenalties();
        }
    }
}
