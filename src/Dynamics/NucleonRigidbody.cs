using NucleonEngine.Calculations;
using NucleonEngine.Collisions;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Dynamics
{
    public class NucleonRigidbody : MonoBehaviour
    {
        [Header("General Settings")]
        public NucleonRigidbodyType BodyType;

        [Header("Physical Settings")]
        public float Mass = 1;
        public float GravityScale = 1;

        [HideInInspector] public svector3 Velocity = svector3.Zero();

        [HideInInspector] public svector3 AbsoluteVelocity = svector3.Zero();
        public bool Grounded { get; private set; }
        public NucleonBoxCollider Ground { get; private set; }

        private Vector3 CalculationVelocity;
        private sfloat GravityTimer;

        private Dictionary<int, bool[]> TouchingFaces = new Dictionary<int, bool[]>();

        void Start()
        {
            Grounded = false;
        }

        void Update()
        {
            GravityTimer = GravityTimer + (sfloat)Time.deltaTime;
        }

        void FixedUpdate()
        {
            if (!Grounded)
            {
                CalculationVelocity.y += (float)(-(sfloat)GravityScale * GravityTimer);
            }

            PrepareVelocity();
            transform.position += (Vector3)(AbsoluteVelocity * (sfloat)Time.fixedDeltaTime);
        }

        public void OnNucleonCollisionEnter(NucleonCollision Collision)
        {
            if (!TouchingFaces.ContainsKey(Collision.OtherCollider.GetInstanceID()))
            {
                TouchingFaces.Add(Collision.OtherCollider.GetInstanceID(), new bool[0]);
            }
            bool[] FacesArray = { Collision.TouchingMinX, Collision.TouchingMaxX, Collision.TouchingMinY, Collision.TouchingMaxY, Collision.TouchingMinZ, Collision.TouchingMaxZ };
            TouchingFaces[Collision.OtherCollider.GetInstanceID()] = FacesArray;

            transform.position = (Vector3)PrepareDetunnel(FacesArray, Collision);

            if (Collision.CollisionDirection.y < (sfloat)0 && !FacesArray[0] && !FacesArray[1] && !FacesArray[4] && !FacesArray[5])
            {
                Grounded = true;
                Ground = Collision.OtherCollider;
                CalculationVelocity.y = 0;
                GravityTimer = (sfloat)0;
                transform.position = (Vector3)new svector3(
                    (sfloat)transform.position.x,
                    (((sfloat)transform.localScale.y + (sfloat)Collision.SelfCollider.Scale.y) / (sfloat)2) + Collision.OtherCollider.CubeModel.MaxY - (sfloat)Collision.SelfCollider.CollisionOverlapThreshold - (sfloat)Collision.SelfCollider.Position.y,
                    (sfloat)transform.position.z);
            }
        }

        public void OnNucleonCollisionExit(NucleonCollisionExit CollisionExit)
        {
            TouchingFaces.Remove(CollisionExit.OtherCollider.GetInstanceID());

            if (CollisionExit.OtherCollider == Ground)
            {
                Grounded = false;
                Ground = null;
                GravityTimer = (sfloat)0;
            }
        }

        private void PrepareVelocity()
        {
            AbsoluteVelocity = (svector3)CalculationVelocity + Velocity;

            foreach (var CollisionSide in TouchingFaces)
            {
                if (CollisionSide.Value[0] == true && AbsoluteVelocity.x > (sfloat)0)
                {
                    AbsoluteVelocity.x = BodyType == 0 ? (sfloat)0 : (sfloat)0;
                }
                if (CollisionSide.Value[1] == true && AbsoluteVelocity.x < (sfloat)0)
                {
                    AbsoluteVelocity.x = BodyType == 0 ? (sfloat)0 : (sfloat)0;
                }

                if (CollisionSide.Value[2] == true && AbsoluteVelocity.y > (sfloat)0)
                {
                    AbsoluteVelocity.y = BodyType == 0 ? (sfloat)0 : (sfloat)0;
                }

                if (CollisionSide.Value[4] == true && AbsoluteVelocity.z > (sfloat)0)
                {
                    AbsoluteVelocity.z = BodyType == 0 ? (sfloat)0 : (sfloat)0;
                }
                if (CollisionSide.Value[5] == true && AbsoluteVelocity.z < (sfloat)0)
                {
                    AbsoluteVelocity.z = BodyType == 0 ? (sfloat)0 : (sfloat)0;
                }
            }
        }

        private svector3 PrepareDetunnel(bool[] FacesArray, NucleonCollision Collision)
        {
            svector3 DetunneledPosition = new svector3(0, 0, 0);
            if (FacesArray[0] && Collision.SelfCollider.DeltaPosition.x != (sfloat)0)
            {
                DetunneledPosition.x = Collision.SelfCollider.CubeModel.MaxX - Collision.OtherCollider.CubeModel.MinX;
            }
            if (FacesArray[1] && Collision.SelfCollider.DeltaPosition.x != (sfloat)0)
            {
                DetunneledPosition.x = Collision.SelfCollider.CubeModel.MinX - Collision.OtherCollider.CubeModel.MaxX;
            }

            if (FacesArray[4] && Collision.SelfCollider.DeltaPosition.z != (sfloat)0)
            {
                DetunneledPosition.z = Collision.SelfCollider.CubeModel.MaxZ - Collision.OtherCollider.CubeModel.MinZ;
            }
            if (FacesArray[5] && Collision.SelfCollider.DeltaPosition.z != (sfloat)0)
            {
                DetunneledPosition.z = Collision.SelfCollider.CubeModel.MinZ - Collision.OtherCollider.CubeModel.MaxZ;
            }
            return new svector3((sfloat)transform.position.x - DetunneledPosition.x, (sfloat)transform.position.y, (sfloat)transform.position.z - DetunneledPosition.z);
        }

        public void AddForce(sfloat Force, svector3 Direction)
        {
            sfloat Acceleration = Force / (sfloat)Mass;
            CalculationVelocity += (Vector3)new svector3(Direction.x * Acceleration, Direction.y * Acceleration, Direction.z * Acceleration);
        }
    }
}