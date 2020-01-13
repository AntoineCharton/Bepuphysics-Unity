//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using BepuPhysicsUnity;

[CustomEditor(typeof(ThreadedSimulation))]
[CanEditMultipleObjects]
public class ThreadedSimulationEditor : Editor
{
    ThreadedSimulation threadedSimulation;

    void OnEnable()
    {
        threadedSimulation = serializedObject.targetObject as ThreadedSimulation;
    }

    public override void OnInspectorGUI()
    {
        if(Application.isPlaying)
        {
            UpdatePlaying();
        } else
        {
            UpdateNotPlaying();
        }
    }

   private void UpdateNotPlaying()
   {
        threadedSimulation.Gravity = EditorGUILayout.Vector3Field("Gravity", threadedSimulation.Gravity);
        threadedSimulation.StepsInterpolation = EditorGUILayout.Slider("Interpolation", threadedSimulation.StepsInterpolation, 0.001f, 1);
        threadedSimulation.TargetTimeStep = Mathf.Max(0.00001f, EditorGUILayout.FloatField("Target timeStep", threadedSimulation.TargetTimeStep));
        
   }

    private void UpdatePlaying()
    {
        threadedSimulation.Gravity = EditorGUILayout.Vector3Field("Gravity", threadedSimulation.Gravity);
        threadedSimulation.TargetTimeStep = Mathf.Max(0.00001f,EditorGUILayout.FloatField("Target timeStep", threadedSimulation.TargetTimeStep));
        threadedSimulation.StepsInterpolation = EditorGUILayout.Slider("Interpolation", threadedSimulation.StepsInterpolation, 0.001f, 1);
        EditorGUILayout.LabelField("Physics processing", threadedSimulation.physicsElapsed + "ms");
        EditorGUILayout.LabelField("Total time", threadedSimulation.totalElapsed + "ms");
    }
}