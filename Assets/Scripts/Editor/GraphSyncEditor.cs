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

        if (GUILayout.Button("EXPORT"))
        {
            graphSync.ExportGraph();
        }

        if(GUILayout.Button("IMPORT"))
        {
            graphSync.ImportGraph();
        }

        if(GUILayout.Button("Update Graph"))
        {
            graphSync.UpdateGraph();
        }
    }
}
