using NucleonEngine.Calculations;
using NucleonEngine.Dynamics;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Collisions
{
    public class NucleonBoxCollider : MonoBehaviour, NucleonCollider
    {
        [Header("General Settings")]
        public bool Trigger;

        [Header("Collider Customization")]
        public Vector3 Position;
        public Vector3 Scale;

        [Header("Specific Settings")]
        public float UpdatesPerSecond = 60;
        public float CollisionOverlapThreshold = 0.001f;

        [Header("Debug Settings")]
        public bool DebugCollisionVertices;
        public bool DebugCenterOfGravity;

        public NucleonRigidbody Body { get; private set; }
        public CubeModel CubeModel { get; private set; }
        public bool Colliding { get; private set; }
        public List<NucleonCollider> ActiveCollisions { get; private set; }
        public List<svector3> CollidingPoints { get; private set; }
        public svector3 DeltaPosition { get; private set; }
        public svector3 LastPosition { get; private set; }

        private NucleonManager NucleonManager;
        private sfloat UpdateInterval;
        private sfloat ElapsedUpdateTime;

        void Awake()
        {
            DeltaPosition = svector3.Zero();
            LastPosition = svector3.Zero();

            try
            {
                Body = GetComponent<NucleonRigidbody>();
            }
            catch { }

            CubeModel = new CubeModel();

            ActiveCollisions = new List<NucleonCollider>();
            CollidingPoints = new List<svector3>();

            UpdateInterval = (sfloat)1f / (sfloat)UpdatesPerSecond;
        }

        void Start()
        {
            NucleonManager = NucleonManager.Instance;
            FetchCollisions();
        }

        void Update()
        {
            ElapsedUpdateTime += (sfloat)Time.deltaTime;
            if(ElapsedUpdateTime >= UpdateInterval)
            {
                ElapsedUpdateTime = ElapsedUpdateTime % UpdateInterval;
                FetchCollisions();
            }
        }

        public void FetchCollisions()
        {
            CubeModel.UpdateBounds(transform, (svector3)Position, (svector3)Scale);
            Colliding = ActiveCollisions.Count > 0;
            DeltaPosition = ((svector3)transform.position + (svector3)Position) - LastPosition;
            LastPosition = ((svector3)transform.position + (svector3)Position);

            for (int i = 0; i < NucleonManager.Colliders.Length; i++)
            {
                NucleonBoxCollider BoxCollider = (NucleonBoxCollider)NucleonManager.Colliders[i];
                if (BoxCollider != this)
                {
                    bool Colliding = intersector.CC_I(CubeModel, BoxCollider.CubeModel);
                    CollisionCheck(Colliding, BoxCollider);

                    if (Colliding && DebugCollisionVertices)
                    {
                        List<svector3> CollidingPointsFetchList = new List<svector3>();
                        foreach (svector3 Vertice in CubeModel.Vertices)
                        {
                            if (intersector.PC_I(Vertice, BoxCollider.CubeModel))
                            {
                                CollidingPointsFetchList.Add(Vertice);
                            }
                        }
                        CollidingPoints = CollidingPointsFetchList;
                    }
                    if (!Colliding && DebugCollisionVertices)
                    {
                        CollidingPoints.Clear();
                    }
                }
            }
        }

        private void CollisionCheck(bool Colliding, NucleonBoxCollider BoxCollider)
        {
            if (Colliding)
            {
                NucleonCollision Collision = new NucleonCollision();
                Collision.SelfCollider = this;
                Collision.OtherCollider = BoxCollider;
                Collision.CollisionDirection = DeltaPosition;

                Collision.TouchingMinX = CubeModel.MaxX >= BoxCollider.CubeModel.MinX && CubeModel.MaxX <= BoxCollider.CubeModel.MinX + (sfloat)0.1f;
                Collision.TouchingMaxX = CubeModel.MinX <= BoxCollider.CubeModel.MaxX && CubeModel.MinX >= BoxCollider.CubeModel.MaxX - (sfloat)0.1f;

                Collision.TouchingMinY = CubeModel.MaxY >= BoxCollider.CubeModel.MinY && CubeModel.MaxY <= BoxCollider.CubeModel.MinY + (sfloat)0.1f;
                Collision.TouchingMaxY = CubeModel.MinY <= BoxCollider.CubeModel.MaxY && CubeModel.MinY >= BoxCollider.CubeModel.MaxY - (sfloat)0.5f;

                Collision.TouchingMinZ = CubeModel.MaxZ >= BoxCollider.CubeModel.MinZ && CubeModel.MaxZ <= BoxCollider.CubeModel.MinZ + (sfloat)0.1f;
                Collision.TouchingMaxZ = CubeModel.MinZ <= BoxCollider.CubeModel.MaxZ && CubeModel.MinZ >= BoxCollider.CubeModel.MaxZ - (sfloat)0.1f;

                if (!ActiveCollisions.Contains(BoxCollider))
                {
                    ActiveCollisions.Add(BoxCollider);
                    Body?.OnNucleonCollisionEnter(Collision);
                }
            }
            else
            {
                if (ActiveCollisions.Contains(BoxCollider))
                {
                    NucleonCollisionExit CollisionExit = new NucleonCollisionExit();
                    CollisionExit.SelfCollider = this;
                    CollisionExit.OtherCollider = BoxCollider;
                    CollisionExit.ExitDirection = DeltaPosition;

                    ActiveCollisions.Remove(BoxCollider);
                    Body?.OnNucleonCollisionExit(CollisionExit);
                }
            }
        }

        void OnDrawGizmos()
        {
            try
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(
                    new Vector3(
                        transform.position.x + Position.x,
                        transform.position.y + Position.y,
                        transform.position.z + Position.z),
                    new Vector3(
                        transform.localScale.x + Scale.x,
                        transform.localScale.y + Scale.y,
                        transform.localScale.z + Scale.z)
                    );

                if (DebugCollisionVertices)
                {
                    Gizmos.color = Color.blue;
                    foreach (svector3 Point in CollidingPoints)
                    {
                        Gizmos.DrawSphere((Vector3)Point, 0.1f);
                    }
                }

                if (DebugCenterOfGravity)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(transform.position, 0.1f);
                }
            }
            catch { }
        }
    }
}