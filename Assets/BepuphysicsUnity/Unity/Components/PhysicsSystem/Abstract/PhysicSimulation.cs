using BepuPhysics;
using UnityEngine;
using BepuphysicsUnity.Utilities;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Collections;
using System.Diagnostics;

namespace BepuPhysicsUnity
{
    /// <summary>
    /// A pyramid of boxes, because you can't have a physics engine without pyramids of boxes.
    /// </summary>
    public abstract class PhysicSimulation : BepuPhysicsUnity
    {
        public Vector3 Gravity;
        public float StepsInterpolation = 0.5f;
        public BodyDescription Body;

        protected void SetupSimulation()
        {
            BodiesData = new List<BodyUpdateData>();
            Simulation = Simulation.Create(BufferPool, new BepuNarrowPhaseCallbacks(), new BepuPoseIntegratorCallbacks(new System.Numerics.Vector3(Gravity.x, -Gravity.y, Gravity.z)));
        }

        protected void UpdatePhysics(float dT)
        {
            UpdatePhysics(null, null, dT);
            lock (BodiesData)
            {
                for (var i = 0; i < BodiesData.Count; i++)
                {
    
                    if (Simulation.Bodies.BodyExists(BodiesData[i].BodieID))
                    {
                        Simulation.Bodies.GetDescription(BodiesData[i].BodieID, out var BodyDescription);
                        BodiesData[i].IsAddedToSimulation = true;
                        BodiesData[i].SetPosition(new Vector3(BodyDescription.Pose.Position.X, BodyDescription.Pose.Position.Y, BodyDescription.Pose.Position.Z));
                        BodiesData[i].SetRotation(new Quaternion(BodyDescription.Pose.Orientation.X, BodyDescription.Pose.Orientation.Y, BodyDescription.Pose.Orientation.Z, BodyDescription.Pose.Orientation.W));
                    } else
                    {
                        BodiesData[i].IsAddedToSimulation = false;
                    }
                }
            }

            lock (AddedBodies)
            {
                foreach (var addedBody in AddedBodies)
                {
                    var id = BodiesFactory.AddBody(Simulation, addedBody.Position,
                        addedBody.Rotation,
                        addedBody.BodyType,
                        addedBody.BodyShape,
                        addedBody.Mass);
                    addedBody.PhysicObjectAddedToSimulation(id);
                }
                AddedBodies.Clear();
            }

            lock (AddedStaticBodies)
            {
                foreach (var addedBody in AddedStaticBodies)
                {
                    var id = BodiesFactory.AddBody(Simulation, addedBody.Position,
                        addedBody.Rotation,
                        addedBody.BodyType,
                        addedBody.BodyShape,
                        addedBody.Mass);
                    addedBody.PhysicObjectAddedToSimulation(id);

                }
                AddedStaticBodies.Clear();

            }
        }

        void UpdateBodiesUnity(int startingCount, int targetCount)
        {

        }

        private void Update()
        {
            lock (BodiesData)
            {
                for (var i = 0; i < BodiesData.Count; i++)
                {
                    if(BodiesData[i].IsAddedToSimulation)
                        BodiesData[i].UpdateBodyLerped(StepsInterpolation);
                }
            }
        }

        public override void UpdatePhysics(Camera camera, Input input, float dt)
        {
            base.UpdatePhysics(camera, input, dt);
        }
    }
}
