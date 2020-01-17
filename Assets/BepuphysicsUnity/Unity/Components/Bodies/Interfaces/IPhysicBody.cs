using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public interface IPhysicsBody
    {
        void AddImpulse(Vector3 force);
        void AddAngularImpulse(Vector3 force);
        Vector3 GetVelocity();
        Vector3 GetAngularVelocity();
    }
}
