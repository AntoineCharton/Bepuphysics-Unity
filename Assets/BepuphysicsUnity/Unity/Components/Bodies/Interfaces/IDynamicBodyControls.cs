using UnityEngine;

namespace BepuPhysicsUnity
{
    public interface IDynamicBodyControls
    {
        void AddImpulse(Vector3 force);
        void AddAngularImpulse(Vector3 force);
        void SetVelocity(Vector3 velocity);
        void SetAngularVelocity(Vector3 velocity);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        Vector3 GetVelocity();
        Vector3 GetAngularVelocity();
    }
}
