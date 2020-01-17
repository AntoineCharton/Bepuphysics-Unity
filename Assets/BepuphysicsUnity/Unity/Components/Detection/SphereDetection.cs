using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public class SphereDetection : DetectionBehaviour, ISphereDetection
    {
        [SerializeField]
        private float Radius = 0.5f;

        public float GetRadius()
        {
            return Radius;
        }

        public void SetRadius(float radius)
        {
            Radius = radius;
        }
    }
}
