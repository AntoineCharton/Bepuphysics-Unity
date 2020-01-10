using BepuPhysics;
using UnityEngine;
using BepuphysicsUnity.Utilities;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Collections;

namespace BepuPhysicsUnity
{
    /// <summary>
    /// A pyramid of boxes, because you can't have a physics engine without pyramids of boxes.
    /// </summary>
    public class PhysicSimulation : BepuPhysicsUnity
    {
        public BodyDescription Body;
        private bool isThreadActive = false;
        Thread thread;
        long oldTime;


        public unsafe override void Initialize()
        {
            //PhysicsGameObjects = new List<GameObject>();
            BodiesData = new List<BodyUpdateData>();
            Simulation = Simulation.Create(BufferPool, new BepuNarrowPhaseCallbacks(), new BepuPoseIntegratorCallbacks(new System.Numerics.Vector3(0, -10, 0)));
            StartThread();
            //StartCoroutine(UpdateObjects());
        }

        private void StartThread()
        {
            isThreadActive = true;
            thread = new Thread(new ThreadStart(ThreadedUpdate));
            thread.Start();
        }

        void UpdatePhysics(float dT)
        {
            UpdatePhysics(null, null, Mathf.Max(0.01f, dT));
            for (var i = 0; i < BodiesData.Count; i++)
            {
                if (Simulation.Bodies.BodyExists(BodiesData[i].BodieID))
                {
                    Simulation.Bodies.GetDescription(BodiesData[i].BodieID, out var BodyDescription);
                    BodiesData[i].SetPosition(new Vector3(BodyDescription.Pose.Position.X, BodyDescription.Pose.Position.Y, BodyDescription.Pose.Position.Z));
                    BodiesData[i].SetRotation(new Quaternion(BodyDescription.Pose.Orientation.X, BodyDescription.Pose.Orientation.Y, BodyDescription.Pose.Orientation.Z, BodyDescription.Pose.Orientation.W));
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



        void ThreadedUpdate()
        {
            try
            {
                while (isThreadActive)
                {
                    const float targetTimeStep = 0.1f;
                    long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    float dT = (now - oldTime) * 0.01f; // / 1000
                    Thread.Sleep(Math.Min(Mathf.Max(0,(int)(100 - (now - oldTime))), 200));
                    oldTime = now;
                    UpdatePhysics(targetTimeStep);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
                throw;
            }
            UnityEngine.Debug.Log("Thread completed");
        }

        private void OnDestroy()
        {
            isThreadActive = false;
            Dispose();
        }

        private void Update()
        {
            //UpdatePhysics();
            lock (BodiesData)
            {
                foreach (var bodyData in BodiesData)
                {
                    bodyData.UpdateBodyLerped(0.2f);
                }
            }
                //ThreadedUpdate();
                //for(var i = 0; i < PhysicsGameObjects.Count; i++)
                //{
                //    Simulation.Bodies.GetDescription(i, out Body);
                //    PhysicsGameObjects[i].transform.position = new UnityEngine.Vector3(Body.Pose.Position.X, Body.Pose.Position.Y, Body.Pose.Position.Z);
                //    PhysicsGameObjects[i].transform.rotation = new UnityEngine.Quaternion(Body.Pose.Orientation.X, Body.Pose.Orientation.Y, Body.Pose.Orientation.Z, Body.Pose.Orientation.W);
                //}
            }

        public override void UpdatePhysics(Camera camera, Input input, float dt)
        {
            base.UpdatePhysics(camera, input, dt);
        }
    }
}
