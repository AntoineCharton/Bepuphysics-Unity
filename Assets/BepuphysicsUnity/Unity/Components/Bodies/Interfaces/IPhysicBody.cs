using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public interface IPhysicsBody
    {
        void AddForce(Vector3 force);
        void AddTorque(Vector3 force);
        Vector3 GetVelocity();
        Vector3 GetAngularVelocity();
    }
}
