using System;

namespace BepuPhysicsUnity
{

    public struct PhysicBodyData
    {
        private System.Numerics.Vector3 position;
        private BepuUtilities.Quaternion rotation;
        private object bodyType;
        private object bodyShape;
        private float mass;
        private BepuPhysicsUnity.PhysicObjectAddedToSimulation physicObjectAddedToSimulation;

        public System.Numerics.Vector3 Position { get => position; set => position = value; }
        public BepuUtilities.Quaternion Rotation { get => rotation; set => rotation = value; }
        public object BodyType { get => bodyType; set => bodyType = value; }
        public object BodyShape { get => bodyShape; set => bodyShape = value; }
        public float Mass { get => mass; set => mass = value; }
        public BepuPhysicsUnity.PhysicObjectAddedToSimulation PhysicObjectAddedToSimulation { get => physicObjectAddedToSimulation; set => physicObjectAddedToSimulation = value; }

        public PhysicBodyData(UnityEngine.Vector3 originalPosition, UnityEngine.Quaternion originalRotation, object addedBodyType, object AddedBodyShape, float OriginalMass, BepuPhysicsUnity.PhysicObjectAddedToSimulation physicObjectAddedToSimulationCallback)
        {
            position = new System.Numerics.Vector3(originalPosition.x, originalPosition.y, originalPosition.z);
            rotation = new BepuUtilities.Quaternion(originalRotation.x, originalRotation.y, originalRotation.z, originalRotation.w);
            bodyType = addedBodyType;
            bodyShape = AddedBodyShape;
            mass = OriginalMass;
            physicObjectAddedToSimulation = physicObjectAddedToSimulationCallback;
        }
    }
}