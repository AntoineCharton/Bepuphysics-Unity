//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using BepuPhysicsUnity;
using System;

[CustomEditor(typeof(SphereDetection))]
[CanEditMultipleObjects]
public class SphereDetectionEditor : Editor
{
    SphereDetection boxDetection;

    void OnEnable()
    {
        boxDetection = serializedObject.targetObject as SphereDetection;
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
        boxDetection.SetRadius(EditorGUILayout.FloatField("Radius", boxDetection.GetRadius()));
        OnDrawGizmosSelected(null);
    }

    void OnDrawGizmosSelected(SceneView scene)
    {
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(boxDetection.transform.position, boxDetection.transform.up, boxDetection.GetRadius());
        Handles.DrawWireDisc(boxDetection.transform.position, boxDetection.transform.right, boxDetection.GetRadius());
        Handles.DrawWireDisc(boxDetection.transform.position, boxDetection.transform.forward, boxDetection.GetRadius());
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
        Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
        Handles.DrawWireDisc(boxDetection.transform.position, boxDetection.transform.up, boxDetection.GetRadius());
        Handles.DrawWireDisc(boxDetection.transform.position, boxDetection.transform.right, boxDetection.GetRadius());
        Handles.DrawWireDisc(boxDetection.transform.position, boxDetection.transform.forward, boxDetection.GetRadius());
        scene?.Repaint();
    }
}
