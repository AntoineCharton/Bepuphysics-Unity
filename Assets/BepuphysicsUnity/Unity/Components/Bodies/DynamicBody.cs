using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity {
    public class DynamicBody : PhysicBody, IDynamicBody, IBodyUpdate
    {
        private List<BepuPhysicsUnity> PhysicsSpaces;
        private DetectionBehaviour Detection;
        [SerializeField]
        private float mass;
        public int ID;
        private BepuPhysicsUnity physicSpace;

        public void OnBodyUpdated(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        private void Awake()
        {
            PhysicsSpaces = new List<BepuPhysicsUnity>(FindObjectsOfType<BepuPhysicsUnity>());
            Detection = GetComponent<DetectionBehaviour>();
            if(PhysicsSpaces.Count > 0)
            {
                PhysicsSpaces[0].AddShape(transform.position, transform.rotation, this, Detection,mass, OnShapeAdded);
                physicSpace = PhysicsSpaces[0];
            }
        }

        private void OnShapeAdded(int id)
        {
            ID = id;
            physicSpace.SubscribePhysicsUpdate(this, id);
        }
    }
}
