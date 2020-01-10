using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public abstract class PhysicsComponent : MonoBehaviour
    {
        private void Awake()
        {
            foreach(var physicsComponent in GetComponents<IPhysicsComponentAdded>())
            {
                physicsComponent.OnPhysicsComponentAdded(this);
            }
        }

        private void OnDestroy()
        {
            foreach (var physicsComponent in GetComponents<IPhysicsComponentDestroyed>())
            {
                physicsComponent.OnPhysicsComponentDestroyed(this);
            }
        }
    }
}
