using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Grid))]
class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Grid gridSystem = (Grid)target;

        if (GUILayout.Button("Recalculate Mesh"))
            gridSystem.RecalculateMesh();
        if (GUILayout.Button("Animate Mesh"))
            gridSystem.ShowAnimation();
    }
}