using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public interface IPhysicsComponentDestroyed
    {
        void OnPhysicsComponentDestroyed(PhysicsComponent component);
    }
}
