using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public class CapsuleDetection : DetectionBehaviour, ICapsuleDetection
    {
        [SerializeField]
        private float Height = 2;
        [SerializeField]
        private float Radius = 0.5f;

        public float GetHeight()
        {
            return Height;
        }

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
            Gizmos.color = Color.cyan;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            GizmoHelper.DrawWireCapsule(transform.position, transform.rotation, Radius, Height, Color.cyan);
        }
    }
}
