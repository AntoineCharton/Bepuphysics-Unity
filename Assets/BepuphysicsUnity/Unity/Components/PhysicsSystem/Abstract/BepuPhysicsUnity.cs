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
        protected List<BodyUpdateData> PhysicUpdates;
        bool isInitialized = false;
        public delegate void PhysicObjectAddedToSimulation(int ID, Vector3 position, Quaternion rotation);
        protected List<PhysicBodyData> AddedBodies;
        protected List<BodyUpdateData> BodiesData;

        protected List<PhysicBodyData> AddedStaticBodies;
        protected List<BodyUpdateData> StaticBodiesData;

        private void Awake()
        {
            InitializePhysics();
        }

        public void SubscribePhysicsUpdate(IBodyUpdate physicsUpdate, int bodieID, Vector3 defaultPosition, Quaternion defaultRotation)
        {
            var physicUpdate = new BodyUpdateData(physicsUpdate, bodieID);
            physicUpdate.SetPosition(defaultPosition, false);
            physicUpdate.SetRotation(defaultRotation, false);
            lock (BodiesData)
            {
                BodiesData.Add(physicUpdate);
            }
        }

        protected void InitializePhysics()
        {
            if (isInitialized)
                return;
           
            AddedBodies = new List<PhysicBodyData>();
            PhysicUpdates = new List<BodyUpdateData>();
            AddedStaticBodies = new List<PhysicBodyData>();
            StaticBodiesData = new List<BodyUpdateData>();
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
            lock (AddedBodies)
            {
                AddedBodies.Add(newBody);
            }
        }

        public void AddStaticShape(Vector3 position, Quaternion rotation, object bodyType, object bodyShape, float mass, PhysicObjectAddedToSimulation physicObjectAddedToSimulation)
        {
            if (!isInitialized)
                InitializePhysics();

            var newBody = new PhysicBodyData(position, rotation, bodyType, bodyShape, mass, physicObjectAddedToSimulation);
            lock (AddedStaticBodies)
            {
                AddedStaticBodies.Add(newBody);
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

    public struct PhysicBodyData
    {
        public System.Numerics.Vector3 Position;
        public BepuUtilities.Quaternion Rotation;
        public object BodyType;
        public object BodyShape;
        public float Mass;
        public BepuPhysicsUnity.PhysicObjectAddedToSimulation PhysicObjectAddedToSimulation;

        public PhysicBodyData(UnityEngine.Vector3 position, UnityEngine.Quaternion rotation, object bodyType, object bodyShape, float mass, BepuPhysicsUnity.PhysicObjectAddedToSimulation physicObjectAddedToSimulation)
        {
            Position = new System.Numerics.Vector3(position.x, position.y, position.z);
            Rotation = new BepuUtilities.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            BodyType = bodyType;
            BodyShape = bodyShape;
            Mass = mass;
            PhysicObjectAddedToSimulation = physicObjectAddedToSimulation;

        }
    }
}