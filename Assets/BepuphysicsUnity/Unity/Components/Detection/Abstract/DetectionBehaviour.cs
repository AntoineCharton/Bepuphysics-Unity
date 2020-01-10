using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity
{
    public abstract class DetectionBehaviour : PhysicsComponent
    {
        public abstract object GetShapeType();
    }
}
