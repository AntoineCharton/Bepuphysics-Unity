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

        public Vector3 GetSize()
        {
            return Size;
        }

        public void SetSize(Vector3 size)
        {
            Size = size;
        }

    }
}
