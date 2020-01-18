using UnityEngine;

namespace BepuPhysicsUnity
{
    public interface IBodyControls
    {
        void SetVelocity(Vector3 velocity);
        void SetAngularVelocity(Vector3 velocity);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
    }
}
