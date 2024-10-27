using System.Collections;
using System.Collections.Generic;
using System;
using BepuPhysics;
using BepuUtilities;
using BepuPhysics.Collidables;
using System.Numerics;
using UnityEngine;

namespace BepuPhysicsUnity {
    public static class BodiesFactory {
        private const string CouldNotInitializeMessage = "shape Type not found could not initilize physics";

        public static BodyHandle AddBody(Simulation simulation, System.Numerics.Vector3 position,  System.Numerics.Quaternion rotation, object bodyType, object shapeType, out bool isStatic, float mass = 1)
        {
            switch (bodyType)
            {
                case IDynamicBody dynamicBody:
                    isStatic = false;
                    return AddDynamicBody(simulation, position, rotation, shapeType, mass);
                case IKinematicBody kinematicBody:
                    isStatic = false;
                    return AddKinematicBody(simulation, position, rotation, shapeType);
                default:
                    throw new System.ArgumentException(message: CouldNotInitializeMessage, nameof(shapeType));
            }
        }
        
        public static StaticHandle AddStaticBody(Simulation simulation, System.Numerics.Vector3 position,  System.Numerics.Quaternion rotation, object bodyType, object shapeType, out bool isStatic, float mass = 1)
        {
            switch (bodyType)
            {
                case IStaticBody staticBody:
                    isStatic = true;
                    return AddStaticBody(simulation, position, rotation, shapeType);
                default:
                    throw new System.ArgumentException(message: CouldNotInitializeMessage, nameof(shapeType));
            }
        }
        
        static StaticHandle AddStaticBody(Simulation simulation, System.Numerics.Vector3 position, System.Numerics.Quaternion rotation, object shapeType)
        {
            switch (shapeType)
            {
                case IBoxDetection boxDetection:
                    IBoxDetection box = shapeType as IBoxDetection;
                    var boxShape = new Box(box.GetSize().x, box.GetSize().y, box.GetSize().z);
                    return simulation.Statics.Add(new StaticDescription(position, rotation,
                        simulation.Shapes.Add(new Box(1, 1, 1))));
                /*case ISphereDetection sphereDetection:
                    ISphereDetection sphere = shapeType as ISphereDetection;
                    var sphereShape = new Sphere(sphere.GetRadius());
                    return simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(position.X, position.Y, position.Z),
                        new BepuUtilities.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W),
                        new CollidableDescription(simulation.Shapes.Add(sphereShape), 0.01f)));
                case ICapsuleDetection capsuleDetection:
                    ICapsuleDetection capsule = shapeType as ICapsuleDetection;
                    var capsuleShape = new Capsule(capsule.GetRadius(), capsule.GetHeight());
                    return simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(position.X, position.Y, position.Z),
                        new BepuUtilities.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W),
                        new CollidableDescription(simulation.Shapes.Add(capsuleShape), 0.01f)));*/
                default:
                    throw new System.ArgumentException(message: CouldNotInitializeMessage, nameof(shapeType));
            }
        }

        static BodyHandle AddDynamicBody(Simulation simulation, System.Numerics.Vector3 position, System.Numerics.Quaternion rotation, object shapeType, float mass)
        {
            
            switch (shapeType)
            {
                case IBoxDetection boxDetection:
                    IBoxDetection box = shapeType as IBoxDetection;
                    var boxInertia = new BodyInertia();
                    var boxShape = new Box(box.GetSize().x, box.GetSize().y, box.GetSize().z);
                    boxShape.ComputeInertia(mass);
                    var boxIndex = simulation.Shapes.Add(boxShape);
                    var boxPose = new RigidPose(position, rotation);
                    var boxHandle = simulation.Bodies.Add(BodyDescription.CreateDynamic(boxPose,
                      boxInertia,
                      new CollidableDescription(boxIndex, 0.01f),
                      new BodyActivityDescription(0.01f)));
                    return boxHandle;
                case ISphereDetection sphereDetection:
                    ISphereDetection sphere = shapeType as ISphereDetection;
                    var sphereInertia = new BodyInertia();
                    var sphereShape = new Sphere(sphere.GetRadius());
                    sphereShape.ComputeInertia(mass);
                    var sphereIndex = simulation.Shapes.Add(sphereShape);
                    var spherePose = new RigidPose(position, rotation);
                    var sphereHandle = simulation.Bodies.Add(BodyDescription.CreateDynamic(spherePose,
                      sphereInertia,
                      new CollidableDescription(sphereIndex, 0.01f),
                      new BodyActivityDescription(0.01f)));
                    return sphereHandle;
                case ICapsuleDetection capsuleDetection:
                    ICapsuleDetection capsule = shapeType as ICapsuleDetection;
                    var capsuleInertia = new BodyInertia();
                    var capsuleShape = new Capsule(capsule.GetRadius(), capsule.GetHeight());
                    capsuleShape.ComputeInertia(mass);
                    var capsuleIndex = simulation.Shapes.Add(capsuleShape);
                    var capsulePose = new RigidPose(position, rotation);
                    var capsuleHandle = simulation.Bodies.Add(BodyDescription.CreateDynamic(capsulePose,
                      capsuleInertia,
                      new CollidableDescription(capsuleIndex, 0.01f),
                      new BodyActivityDescription(0.01f)));
                    return capsuleHandle;
                default:
                    throw new ArgumentException(message: CouldNotInitializeMessage, nameof(shapeType));
            }
        }

        static BodyHandle AddKinematicBody(Simulation simulation, System.Numerics.Vector3 position, System.Numerics.Quaternion rotation, object shapeType)
        {
            switch (shapeType)
            {
                case IBoxDetection boxDetection:
                    IBoxDetection box = shapeType as IBoxDetection;
                    var boxShape = new Box(box.GetSize().x, box.GetSize().y, box.GetSize().z);
                    var boxIndex = simulation.Shapes.Add(boxShape);
                    var boxPose = new RigidPose(position, rotation);
                    var boxeHandle = simulation.Bodies.Add(BodyDescription.CreateKinematic(boxPose,
                      new CollidableDescription(boxIndex, 0.01f),
                      new BodyActivityDescription(0.01f)));
                    return boxeHandle;
                case ISphereDetection sphereDetection:
                    ISphereDetection sphere = shapeType as ISphereDetection;
                    var sphereShape = new Sphere(sphere.GetRadius());
                    var sphereIndex = simulation.Shapes.Add(sphereShape);
                    var spherePose = new RigidPose(position, rotation);
                    var sphereHandle = simulation.Bodies.Add(BodyDescription.CreateKinematic(spherePose,
                      new CollidableDescription(sphereIndex, 0.01f),
                      new BodyActivityDescription(0.01f)));
                    return sphereHandle;
                case ICapsuleDetection capsuleDetection:
                    ICapsuleDetection capsule = shapeType as ICapsuleDetection;
                    var capsuleShape = new Capsule(capsule.GetRadius(), capsule.GetHeight());
                    var capsuleIndex = simulation.Shapes.Add(capsuleShape);
                    var capsulePose = new RigidPose(position, rotation);
                    var capsuleHandle = simulation.Bodies.Add(BodyDescription.CreateKinematic(capsulePose,
                      new CollidableDescription(capsuleIndex, 0.01f),
                      new BodyActivityDescription(0.01f)));
                    return capsuleHandle;
                default:
                    throw new System.ArgumentException(message: CouldNotInitializeMessage, nameof(shapeType));
            }
        }
    }
}
