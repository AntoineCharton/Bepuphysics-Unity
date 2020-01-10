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
        public float StepsInterpolation = 20f;
        public BodyDescription Body;

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
                        BodiesData[i].SetPosition(new Vector3(BodyDescription.Pose.Position.X, BodyDescription.Pose.Position.Y, BodyDescription.Pose.Position.Z));
                        BodiesData[i].SetRotation(new Quaternion(BodyDescription.Pose.Orientation.X, BodyDescription.Pose.Orientation.Y, BodyDescription.Pose.Orientation.Z, BodyDescription.Pose.Orientation.W));
                    } else
                    {
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
                    addedBody.PhysicObjectAddedToSimulation(id,
                        new Vector3(addedBody.Position.X, addedBody.Position.Y, addedBody.Position.Z), 
                        new Quaternion(addedBody.Rotation.X, addedBody.Rotation.Y, addedBody.Rotation.Z, addedBody.Rotation.W));
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
                    addedBody.PhysicObjectAddedToSimulation(id,
                        new Vector3(addedBody.Position.X, addedBody.Position.Y, addedBody.Position.Z),
                        new Quaternion(addedBody.Rotation.X, addedBody.Rotation.Y, addedBody.Rotation.Z, addedBody.Rotation.W));

                }
                AddedStaticBodies.Clear();

            }
        }

        private void Update()
        {
            lock (BodiesData)
            {
                foreach (var bodyData in BodiesData)
                {
                    bodyData.UpdateBodyLerped(StepsInterpolation);
                }
            }
        }

        public override void UpdatePhysics(Camera camera, Input input, float dt)
        {
            base.UpdatePhysics(camera, input, dt);
        }
    }
}
