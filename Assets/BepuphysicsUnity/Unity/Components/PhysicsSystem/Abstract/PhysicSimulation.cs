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
        [SerializeField]
        private Vector3 Gravity = new Vector3(0,10,0);
        [SerializeField]
        private float StepsInterpolation = 0.5f;

        public void SetGravity(Vector3 gravity)
        {
            Gravity = gravity;
        }

        public Vector3 GetGravity()
        {
            return Gravity;
        }

        public void SetStepsInterpolation(float stepsInterpolation)
        {
            StepsInterpolation = stepsInterpolation;
        }

        public float GetStepsInterpolation()
        {
            return StepsInterpolation;
        }

        protected void SetupSimulation()
        {
            SetBodiesData(new List<BodyUpdateData>());
            Simulation = Simulation.Create(BufferPool, new BepuNarrowPhaseCallbacks(), new BepuPoseIntegratorCallbacks(new System.Numerics.Vector3(Gravity.x, -Gravity.y, Gravity.z)));
        }

        protected void UpdatePhysics(float dT)
        {
            lock (GetDynamicBodiesToRemoveID())
            {
                foreach(var body in GetDynamicBodiesToRemoveID())
                {
                    lock (GetBodiesData())
                    {
                        lock (Simulation)
                        {
                            Simulation.Bodies.Remove(body);
                        }
                        for (int i = 0; i < GetBodiesData().Count; i++)
                        {
                            if (GetBodiesData()[i].BodieID == body)
                            {
                                GetBodiesData().RemoveAt(i);
                            }
                        }
                    }
                }
                GetDynamicBodiesToRemoveID().Clear();
            }

            lock (GetStaticBodiesToRemoveID())
            {
                foreach (var body in GetStaticBodiesToRemoveID())
                {
                    lock (GetStaticBodiesData())
                    {
                        lock (Simulation)
                        {
                            Simulation.Statics.Remove(body);
                        }
                        for (int i = 0; i < GetStaticBodiesData().Count; i++)
                        {
                            if (GetStaticBodiesData()[i].BodieID == body)
                            {
                                GetStaticBodiesData().RemoveAt(i);
                            }
                        }
                    }
                }
                GetStaticBodiesToRemoveID().Clear();
            }

            UpdatePhysics(null, null, dT);
            lock (GetBodiesData())
            {
                for (var i = 0; i < GetBodiesData().Count; i++)
                {
                    if (Simulation.Bodies.BodyExists(GetBodiesData()[i].BodieID))
                    {
                        Simulation.Bodies.GetDescription(GetBodiesData()[i].BodieID, out var BodyDescription);
                        GetBodiesData()[i].IsAddedToSimulation = true;
                        GetBodiesData()[i].SetPosition(new Vector3(BodyDescription.Pose.Position.X, BodyDescription.Pose.Position.Y, BodyDescription.Pose.Position.Z));
                        GetBodiesData()[i].SetRotation(new Quaternion(BodyDescription.Pose.Orientation.X, BodyDescription.Pose.Orientation.Y, BodyDescription.Pose.Orientation.Z, BodyDescription.Pose.Orientation.W));
                    } else
                    {
                        GetBodiesData()[i].IsAddedToSimulation = false;
                    }
                }
            }

            lock (GetAddedBodies())
            {
                foreach (var addedBody in GetAddedBodies())
                {
                    var id = BodiesFactory.AddBody(Simulation, addedBody.Position,
                        addedBody.Rotation,
                        addedBody.BodyType,
                        addedBody.BodyShape,
                        addedBody.Mass);
                    addedBody.PhysicObjectAddedToSimulation(id);
                }
                GetAddedBodies().Clear();
            }

            lock (GetAddedStaticBodies())
            {
                foreach (var addedBody in GetAddedStaticBodies())
                {
                    var id = BodiesFactory.AddBody(Simulation, addedBody.Position,
                        addedBody.Rotation,
                        addedBody.BodyType,
                        addedBody.BodyShape,
                        addedBody.Mass);
                    addedBody.PhysicObjectAddedToSimulation(id);

                }
                GetAddedStaticBodies().Clear();

            }
        }

        private void Update()
        {
            lock (GetBodiesData())
            {
                for (var i = 0; i < GetBodiesData().Count; i++)
                {
                    if(GetBodiesData()[i].IsAddedToSimulation)
                        GetBodiesData()[i].UpdateBodyLerped(StepsInterpolation);
                }
            }
        }

        public override void UpdatePhysics(Camera camera, Input input, float dt)
        {
            base.UpdatePhysics(camera, input, dt);
        }
    }
}
