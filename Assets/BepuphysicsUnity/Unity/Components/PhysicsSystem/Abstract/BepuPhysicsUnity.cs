using BepuUtilities.Memory;
using BepuPhysics;
using System;
using UnityEngine;
using BepuPhysicsUnity.Utilities;
using System.Collections.Generic;
using BepuPhysics.Collidables;

namespace BepuPhysicsUnity
{
    public abstract class BepuPhysicsUnity : MonoBehaviour, IDisposable
    {
        /// <summary>
        /// Gets the simulation created by the demo's Initialize call.
        /// </summary>
        public Simulation Simulation { get; protected set; }

        /// <summary>
        /// Gets the buffer pool used by the demo's simulation.
        /// </summary>
        public BufferPool BufferPool { get; private set; }

        /// <summary>
        /// Gets the thread dispatcher available for use by the simulation.
        /// </summary>
        public SimpleThreadDispatcher ThreadDispatcher { get; private set; }

        private List<PhysicBodyData> addedStaticBodies;
        protected List<PhysicBodyData> GetAddedStaticBodies() { return addedStaticBodies; }
        protected void SetAddedStaticBodies(List<PhysicBodyData> value) { addedStaticBodies = value; }

        private List<BodyUpdateData> staticBodiesData;
        protected List<BodyUpdateData> GetStaticBodiesData() { return staticBodiesData; }
        protected void SetStaticBodiesData(List<BodyUpdateData> value) { staticBodiesData = value; }

        private List<BodyUpdateData> bodiesData;
        protected List<BodyUpdateData> GetBodiesData() { return bodiesData;}
        protected void SetBodiesData(List<BodyUpdateData> value) { bodiesData = value; }

        private List<PhysicBodyData> addedBodies;
        protected List<PhysicBodyData> GetAddedBodies() { return addedBodies; }
        protected void SetAddedBodies(List<PhysicBodyData> value){ addedBodies = value;}

        private List<int> dynamicBodyToRemove;
        protected List<int> GetDynamicBodiesToRemove() { return dynamicBodyToRemove; }
        protected void SetDynamicBodiesToRemove(List<int> id) { dynamicBodyToRemove = id; }

        private List<int> staticBodiesToRemove;
        protected List<int> GetStaticBodiesToRemove() { return staticBodiesToRemove; }
        protected void SetStaticBodiesToRemove(List<int> id) { staticBodiesToRemove = id; }

        private List<BodyUpdateData> physicUpdates;
        protected List<BodyUpdateData> GetPhysicUpdates() { return physicUpdates; }
        protected void SetPhysicUpdates(List<BodyUpdateData> value) { physicUpdates = value; }

        bool isInitialized = false;
        public delegate void PhysicObjectAddedToSimulation(int ID);

        private void Awake()
        {
            InitializePhysics();
        }

        public void SubscribePhysicsUpdate(IBodyUpdate physicsUpdate, int bodieID)
        {
            var physicUpdate = new BodyUpdateData(physicsUpdate, bodieID);
            lock (GetBodiesData())
            {
                GetBodiesData().Add(physicUpdate);
            }
        }

        protected void InitializePhysics()
        {
            if (isInitialized)
                return;

            SetAddedBodies(new List<PhysicBodyData>());
            SetPhysicUpdates(new List<BodyUpdateData>());
            SetAddedStaticBodies(new List<PhysicBodyData>());
            SetStaticBodiesData(new List<BodyUpdateData>());
            SetDynamicBodiesToRemove(new List<int>());
            SetStaticBodiesToRemove(new List<int>());
            isInitialized = true;
            BufferPool = new BufferPool();
            ThreadDispatcher = new SimpleThreadDispatcher(Environment.ProcessorCount);
            Initialize();
        }

        public abstract void Initialize();


        public virtual void UpdatePhysics(Camera camera, Input input, float dt)
        {
            Simulation.Timestep(dt, ThreadDispatcher);
        }

        protected virtual void OnDispose()
        {

        }

        public void AddShape(Vector3 position, Quaternion rotation, object bodyType, object bodyShape, float mass, PhysicObjectAddedToSimulation physicObjectAddedToSimulation)
        {
            if (!isInitialized)
                InitializePhysics();

            var newBody = new PhysicBodyData(position, rotation, bodyType, bodyShape, mass, physicObjectAddedToSimulation);
            lock (GetAddedBodies())
            {
                GetAddedBodies().Add(newBody);
            }
        }

        public void AddStaticShape(Vector3 position, Quaternion rotation, object bodyType, object bodyShape, float mass, PhysicObjectAddedToSimulation physicObjectAddedToSimulation)
        {
            if (!isInitialized)
                InitializePhysics();

            var newBody = new PhysicBodyData(position, rotation, bodyType, bodyShape, mass, physicObjectAddedToSimulation);
            lock (GetAddedStaticBodies())
            {
                GetAddedStaticBodies().Add(newBody);
            }
        }

        public void RemoveStaticBody(int ID)
        {
            lock (staticBodiesToRemove)
            {
                staticBodiesToRemove.Add(ID);
            }
        }

        public void RemoveDynamicBody(int ID)
        {
            lock (dynamicBodyToRemove)
            {
                dynamicBodyToRemove.Add(ID);
            }
        }

        bool disposed;
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                OnDispose();
                Simulation.Dispose();
                BufferPool.Clear();
                ThreadDispatcher.Dispose();
            }
        }
    }
}