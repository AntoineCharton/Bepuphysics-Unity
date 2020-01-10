using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity {
    public interface IPhysicsComponentAdded
    {
        void OnPhysicsComponentAdded(PhysicsComponent component);
    }
}
