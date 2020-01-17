//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using BepuPhysicsUnity;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(DynamicBody))]
[CanEditMultipleObjects]
public class DynamicBodyEditor : Editor
{
    DynamicBody boxDetection;

    void OnEnable()
    {
        boxDetection = serializedObject.targetObject as DynamicBody;
    }

    public override void OnInspectorGUI()
    {
        var PhysicsSpaces = new List<BepuPhysicsUnity.BepuPhysicsUnity>(FindObjectsOfType<BepuPhysicsUnity.BepuPhysicsUnity>());

        if(PhysicsSpaces.Count == 0 && boxDetection.isActiveAndEnabled)
        {
            EditorGUILayout.HelpBox("No physic simulation found", MessageType.Error, true);
        }

        if (boxDetection.GetComponent<DetectionBehaviour>() == null)
        {
            EditorGUILayout.HelpBox("No physic detection component attached", MessageType.Error, true);
        }
        boxDetection.SetMass(EditorGUILayout.FloatField("Mass", boxDetection.GetMass()));
    }
}
