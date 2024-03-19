using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GraphSync))]
public class GraphSyncEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GraphSync graphSync = (GraphSync)target;
        if (GUILayout.Button("Get All Nodes"))
        {
            graphSync.GetAllNodes();
        }

        if (GUILayout.Button("Export Graph to json"))
        {
            graphSync.ExportGraph();
        }
    }
}
