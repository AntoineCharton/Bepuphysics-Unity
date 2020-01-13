using BepuPhysics;
using UnityEngine;
using BepuphysicsUnity.Utilities;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Collections;
using System.Diagnostics;

namespace BepuPhysicsUnity {
    public class ThreadedSimulation : PhysicSimulation
    {
        [Range(0.01f, 0.5f)]
        public float TargetTimeStep = 0.1f;
        public long physicsElapsed = 0;
        public long totalElapsed = 0;
        Thread thread;
        private bool isThreadActive = false;

        public unsafe override void Initialize()
        {
            SetupSimulation();
            StartThread();
        }


        private void StartThread()
        {
            isThreadActive = true;
            thread = new Thread(new ThreadStart(ThreadedUpdate));
            thread.Start();
        }

        void ThreadedUpdate()
        {
            try
            {
                while (isThreadActive)
                {
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
            Dispose();
        }

        private void OnDestroy()
        {
            isThreadActive = false;
        }
    }
}
