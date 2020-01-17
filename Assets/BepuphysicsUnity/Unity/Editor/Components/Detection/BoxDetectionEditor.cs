//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using BepuPhysicsUnity;
using System;

[CustomEditor(typeof(BoxDetection))]
[CanEditMultipleObjects]
public class BoxDetectionEditor : Editor
{
    BoxDetection boxDetection;

    void OnEnable()
    {
        boxDetection = serializedObject.targetObject as BoxDetection;
        SceneView.duringSceneGui += OnDrawGizmosSelected;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnDrawGizmosSelected;
    }

    public override void OnInspectorGUI()
    {
        if (boxDetection.GetComponent<PhysicBody>() == null)
        {
            EditorGUILayout.HelpBox("No physic body component attached. Box detection will not be added to Bepu simulation", MessageType.Warning, true);
        }
        boxDetection.SetSize(EditorGUILayout.Vector3Field("Size", boxDetection.GetSize()));
        OnDrawGizmosSelected(null);
    }

    void OnDrawGizmosSelected(SceneView scene)
    {
        Handles.color = Color.cyan;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxDetection.transform.position, boxDetection.transform.rotation, Vector3.one);
        Handles.matrix = rotationMatrix;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
        Handles.DrawWireCube(Vector3.zero, boxDetection.GetSize());
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
        Handles.DrawWireCube(Vector3.zero, boxDetection.GetSize());
        scene?.Repaint();
    }
}
