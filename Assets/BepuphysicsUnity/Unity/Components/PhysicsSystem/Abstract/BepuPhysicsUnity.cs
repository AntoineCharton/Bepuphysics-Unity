using BepuUtilities.Memory;
using BepuPhysics;
using System;
using UnityEngine;
using BepuPhysicsUnity.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private List<int> dynamicBodyToRemoveID;
        protected List<int> GetDynamicBodiesToRemoveID() { return dynamicBodyToRemoveID; }
        protected void SetDynamicBodiesToRemoveID(List<int> id) { dynamicBodyToRemoveID = id; }

        private List<int> staticBodiesToRemoveID;
        protected List<int> GetStaticBodiesToRemoveID() { return staticBodiesToRemoveID; }
        protected void SetStaticBodiesToRemoveID(List<int> id) { staticBodiesToRemoveID = id; }

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
            SetDynamicBodiesToRemoveID(new List<int>());
            SetStaticBodiesToRemoveID(new List<int>());
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

        public void AddImpulse(Vector3 force, int ID)
        {
            lock (Simulation)
            {
                Simulation.Awakener.AwakenBody(ID);
                Simulation.Bodies.GetBodyReference(ID).ApplyImpulse(new System.Numerics.Vector3(force.x, force.y, force.z), System.Numerics.Vector3.Zero);
            }
        }


        public void AddAngularImpulse(Vector3 force, int ID)
        {
            lock (Simulation)
            {
                Simulation.Awakener.AwakenBody(ID);
                Simulation.Bodies.GetBodyReference(ID).ApplyAngularImpulse(new System.Numerics.Vector3(force.x, force.y, force.z));
            }
        }

        public void SetPosition(Vector3 position, int ID)
        {
            lock (Simulation)
            {
                Simulation.Bodies.GetBodyReference(ID).GetDescription(out var description);
                description.Pose.Position = new System.Numerics.Vector3(position.x, position.y, position.z);
                Simulation.Bodies.GetBodyReference(ID).ApplyDescription(description);
                lock (GetBodiesData())
                {
                    Parallel.ForEach(GetBodiesData(), bodyData =>
                    {
                        if (ID == bodyData.BodieID)
                        {
                            bodyData.SetPosition(position, false);
                        }
                    });
                }
            }
        }

        public void SetRotation(Quaternion rotation, int ID)
        {
            lock (Simulation)
            {
                Simulation.Bodies.GetBodyReference(ID).GetDescription(out var description);
                description.Pose.Orientation = new BepuUtilities.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
                Simulation.Bodies.GetBodyReference(ID).ApplyDescription(description);
                lock (GetBodiesData())
                {
                    Parallel.ForEach(GetBodiesData(), bodyData =>
                    {
                        if (ID == bodyData.BodieID)
                        {
                            bodyData.SetRotation(rotation, false);
                        }
                    });
                }
            }
        }

        public void SetAngularVelocity(Vector3 velocity, int ID)
        {
            lock (Simulation)
            {
                Simulation.Bodies.GetBodyReference(ID).GetDescription(out var description);
                description.Velocity.Angular = new System.Numerics.Vector3(velocity.x, velocity.y, velocity.z);
                Simulation.Bodies.GetBodyReference(ID).ApplyDescription(description);
            }
        }

        public void SetVelocity(Vector3 velocity, int ID)
        {
            lock (Simulation)
            {
                Simulation.Bodies.GetBodyReference(ID).GetDescription(out var description);
                description.Velocity.Linear = new System.Numerics.Vector3(velocity.x, velocity.y, velocity.z);
                Simulation.Bodies.GetBodyReference(ID).ApplyDescription(description);
            }
        }

        public Vector3 GetVelocity(int ID)
        {
            lock (Simulation)
            {
                Simulation.Bodies.GetBodyReference(ID).GetDescription(out var description);
                return new Vector3(description.Velocity.Linear.X, description.Velocity.Linear.Y, description.Velocity.Linear.Z);
            }
        }

        public Vector3 GetAngularVelocity(int ID)
        {
            lock (Simulation)
            {
                Simulation.Bodies.GetBodyReference(ID).GetDescription(out var description);
                return new Vector3(description.Velocity.Angular.X, description.Velocity.Angular.Y, description.Velocity.Angular.Z);
            }
        }

        public void RemoveStaticBody(int ID)
        {
            lock (staticBodiesToRemoveID)
            {
                staticBodiesToRemoveID.Add(ID);
            }
        }

        public void RemoveDynamicBody(int ID)
        {
            lock (dynamicBodyToRemoveID)
            {
                dynamicBodyToRemoveID.Add(ID);
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