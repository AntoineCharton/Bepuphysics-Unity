using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public class BoxDetection : DetectionBehaviour, IBoxDetection
    {
        [SerializeField]
        private Vector3 Size = Vector3.one;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Size);
        }

        public Vector3 GetSize()
        {
            return Size;
        }

    }
}
