using NucleonEngine.Calculations;
using NucleonEngine.Dynamics;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Collisions
{
    public class BoxCollider : MonoBehaviour, Collider
    {
        [Header("General Settings")]
        public bool Trigger;
        public NucleonMaterial NucleonMaterial;

        [Header("Collider Customization")]
        public Vector3 Position;
        public Vector3 Scale;

        [Header("Specific Settings")]
        public float CollisionOverlapThreshold = 0.001f;

        [Header("Debug Settings")]
        public bool DebugCollisionVertices;
        public bool DebugCenterOfGravity;

        public PhysicsBody Body { get; private set; }
        public CubeModel CubeModel { get; private set; }
        public bool Colliding { get; private set; }
        public List<Collider> ActiveCollisions { get; private set; }
        public List<svector3> CollidingPoints { get; private set; }
        public svector3 DeltaPosition { get; private set; }
        public svector3 LastPosition { get; private set; }

        private PhysicsManager PhysicsManager;
        private CubeModel CollisionPartner;

        void Awake()
        {
            DeltaPosition = svector3.Zero();
            LastPosition = svector3.Zero();

            try
            {
                Body = GetComponent<PhysicsBody>();
            }
            catch { }
        }

        void Start()
        {
            CubeModel = new CubeModel();
            CubeModel.UpdateBounds(transform, (svector3)Position, (svector3)Scale);

            ActiveCollisions = new List<Collider>();
            CollidingPoints = new List<svector3>();

            PhysicsManager = PhysicsManager.Instance;
            PhysicsManager.RegisterCollider(this);
        }

        void Update()
        {
            CubeModel.UpdateBounds(transform, (svector3)Position, (svector3)Scale);
            Colliding = ActiveCollisions.Count > 0;
            DeltaPosition = ((svector3)transform.position + (svector3)Position) - LastPosition;
            LastPosition = ((svector3)transform.position + (svector3)Position);

            FetchCollisions();
        }

        public void FetchCollisions()
        {
            foreach (BoxCollider BoxCollider in PhysicsManager.Colliders)
            {
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
                        CollidingPoints = new List<svector3>();
                    }
                }
            }
        }

        private void CollisionCheck(bool Colliding, BoxCollider BoxCollider)
        {
            if (Colliding)
            {
                Collision Collision = new Collision();
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
                    CollisionExit CollisionExit = new CollisionExit();
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