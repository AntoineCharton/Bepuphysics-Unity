using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public class SphereDetection : DetectionBehaviour, ISphereDetection
    {
        [SerializeField]
        private float Radius = 1;

        public float GetRadius()
        {
            return Radius;
        }

        public override object GetShapeType()
        {
            return this;
        }

        void OnDrawGizmosSelected()
        {
            const float divideBy2 = 0.5f;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, Radius * divideBy2);
        }
    }
}
