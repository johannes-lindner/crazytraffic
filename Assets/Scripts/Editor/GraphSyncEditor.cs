using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

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

        if(GUILayout.Button("Import GraphConfig.json"))
        {
            graphSync.ImportGraphConfig();
        }

        if(GUILayout.Button("Create Graph"))
        {
            graphSync.CreateGraph();
        }

    }
    //public override VisualElement CreateInspectorGUI()
    //{
    //    // VisualElement myInspector = base.CreateInspectorGUI();
    //    VisualElement myInspector = new VisualElement();
    //
    //    myInspector.Add(new Label("This is a custom inspector"));
    //
    //    return myInspector;
    //}

}
