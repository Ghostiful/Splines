using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Spline spline = (Spline)target;
        if (DrawDefaultInspector())
        {

        }
        
        if (GUILayout.Button("Add Curve"))
        {
            spline.AddCurve();
        }
    }
}
