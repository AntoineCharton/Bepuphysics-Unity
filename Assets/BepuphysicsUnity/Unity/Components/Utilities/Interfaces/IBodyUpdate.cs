using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public interface IBodyUpdate
    {
        void OnBodyUpdated(Vector3 position, Quaternion rotation);
    }
}
