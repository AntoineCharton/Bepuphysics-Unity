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
    public class PhysicSimulation : BepuPhysicsUnity
    {
        [Range(0.01f, 0.5f)]
        public float TargetTimeStep = 0.1f;
        public float StepsInterpolation = 20f;
        public long physicsElapsed = 0;
        public long totalElapsed = 0;
        public BodyDescription Body;
        private bool isThreadActive = false;
        Thread thread;
        long oldTime;
        float TimeScale;


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
            UpdatePhysics(null, null, dT);
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
                    const int oneSecond = 1000;
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    UpdatePhysics(TargetTimeStep);
                    var timeRunned = stopWatch.ElapsedMilliseconds;
                    physicsElapsed = timeRunned;
                    Thread.Sleep(Convert.ToInt32(Mathf.Max(0, (TargetTimeStep * 1000) - timeRunned)));
                    totalElapsed = stopWatch.ElapsedMilliseconds;
                    stopWatch.Stop();
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
            TimeScale = Time.timeScale;
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
