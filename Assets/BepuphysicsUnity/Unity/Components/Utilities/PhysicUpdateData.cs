using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BepuPhysicsUnity {
    public class BodyUpdateData
    {
        public IBodyUpdate PhysicsUpdate;
        public int BodieID;
        public Vector3 TargetPosition;
        public Vector3 CurrentPosition;
        public Quaternion TargetRotation;
        public Quaternion CurrentRotation;

        public BodyUpdateData(IBodyUpdate physicsUpdate, int bodieID, Vector3 position, Quaternion rotation) 
        {
            PhysicsUpdate = physicsUpdate;
            BodieID = bodieID;
            TargetPosition = position;
            TargetRotation = rotation;
        }

        public BodyUpdateData(IBodyUpdate physicsUpdate, int bodieID)
        {
            PhysicsUpdate = physicsUpdate;
            BodieID = bodieID;
            TargetPosition = Vector3.zero;
            TargetRotation = Quaternion.identity;
        }

        public void UpdateBody()
        {
            PhysicsUpdate.OnBodyUpdated(TargetPosition, TargetRotation);
        }

        public void UpdateBodyLerped(float value)
        {
            CurrentPosition = Vector3.Lerp(CurrentPosition, TargetPosition, value * Time.deltaTime);
            CurrentRotation = Quaternion.Lerp(CurrentRotation, TargetRotation, value * Time.deltaTime);
            PhysicsUpdate.OnBodyUpdated(CurrentPosition, TargetRotation);
        }

        public void SetPosition(Vector3 position, bool lerpValue = true)
        {
            TargetPosition = position;
            if(!lerpValue)
            {
                CurrentPosition = position;
            }
        }

        public void SetRotation(Quaternion rotation, bool lerpValue = true)
        {
            TargetRotation = rotation;
            if (!lerpValue)
            {
                TargetRotation = rotation;
            }
        }
    }
}
