using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(S_CheckPointManager))]
public class S_CheckPointManagerEditor : Editor
{
    private S_CheckPointManager script;
    private void OnEnable()
    {
        script = (S_CheckPointManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Sort list to Hierarchy"))
        {
            // Replace original list with sorted version
            script.SortList();
        }
        
        DrawDefaultInspector();
    }
}
