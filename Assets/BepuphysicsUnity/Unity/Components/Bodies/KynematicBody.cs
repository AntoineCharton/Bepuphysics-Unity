using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public class KynematicBody : PhysicBody, IKinematicBody, IBodyUpdate, IBodyControls
    {
        private List<BepuPhysicsUnity> PhysicsSpaces;
        private DetectionBehaviour Detection;
        [SerializeField]
        private float Mass = 1;
        private int ID = -1;
        private BepuPhysicsUnity physicSpace;

        public void OnBodyUpdated(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        void OnEnable()
        {
            PhysicsSpaces = new List<BepuPhysicsUnity>(FindObjectsOfType<BepuPhysicsUnity>());
            Detection = GetComponent<DetectionBehaviour>();
            if (PhysicsSpaces.Count > 0)
            {
                PhysicsSpaces[0].AddShape(transform.position, transform.rotation, this, Detection, Mass, OnShapeAdded);
                physicSpace = PhysicsSpaces[0];
            }
        }

        void OnDisable()
        {
            if (ID != -1)
                physicSpace.RemoveDynamicBody(ID);
        }

        public float GetMass()
        {
            return Mass;
        }

        public void SetMass(float mass)
        {
            Mass = mass;
        }

        public void SetPosition(Vector3 position)
        {
            physicSpace.SetPosition(position, ID);
        }

        public void SetRotation(Quaternion rotation)
        {
            physicSpace.SetRotation(rotation, ID);
        }

        public void SetAngularVelocity(Vector3 velocity)
        {
            physicSpace.SetAngularVelocity(velocity, ID);
        }

        public void SetVelocity(Vector3 velocity)
        {
            physicSpace.SetVelocity(velocity, ID);
        }

        public Vector3 GetVelocity()
        {
            return physicSpace.GetVelocity(ID);
        }

        public Vector3 GetAngularVelocity()
        {
            return physicSpace.GetAngularVelocity(ID);
        }

        private void OnShapeAdded(int id)
        {
            ID = id;
            physicSpace.SubscribePhysicsUpdate(this, id);
        }

        public int GetID()
        {
            return ID;
        }
    }
}
