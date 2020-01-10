using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public class StaticBody : PhysicBody, IStaticBody
    {
        private List<BepuPhysicsUnity> PhysicsSpaces;
        private DetectionBehaviour Detection;
        [SerializeField]
        private int ID;
        private BepuPhysicsUnity physicSpace;

        private void Awake()
        {
            PhysicsSpaces = new List<BepuPhysicsUnity>(FindObjectsOfType<BepuPhysicsUnity>());
            Detection = GetComponent<DetectionBehaviour>();
            if (PhysicsSpaces.Count > 0)
            {
                PhysicsSpaces[0].AddStaticShape(transform.position, transform.rotation, this, Detection.GetShapeType(), 0, OnShapeAdded);
                physicSpace = PhysicsSpaces[0];
            }
        }

        private void OnShapeAdded(int id)
        {
            ID = id;
            //physicSpace.SubscribePhysicsUpdate(this, id);
        }
    }
}
