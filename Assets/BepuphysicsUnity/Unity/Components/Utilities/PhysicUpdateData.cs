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
        public bool IsAddedToSimulation;
        private bool isFirstBodyUpdate = true;

        public BodyUpdateData(IBodyUpdate physicsUpdate, int bodieID, Vector3 position, Quaternion rotation) 
        {
            PhysicsUpdate = physicsUpdate;
            BodieID = bodieID;
            TargetPosition = position;
            TargetRotation = rotation;
            IsAddedToSimulation = false;
        }

        public BodyUpdateData(IBodyUpdate physicsUpdate, int bodieID)
        {
            PhysicsUpdate = physicsUpdate;
            BodieID = bodieID;
            TargetPosition = Vector3.zero;
            TargetRotation = Quaternion.identity;
            IsAddedToSimulation = false;
        }

        public void UpdateBody()
        {
            PhysicsUpdate.OnBodyUpdated(TargetPosition, TargetRotation);
        }

        public void UpdateBodyLerped(float value)
        {
            if (!isFirstBodyUpdate)
            {
                CurrentPosition = Vector3.Lerp(CurrentPosition, TargetPosition, value);
                CurrentRotation = Quaternion.Lerp(CurrentRotation, TargetRotation, value);
            } else
            {
                CurrentPosition = TargetPosition;
                CurrentRotation = TargetRotation;
                isFirstBodyUpdate = false;
            }
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
