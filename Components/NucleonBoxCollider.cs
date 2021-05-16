using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonBoxCollider : MonoBehaviour, NucleonCollider
{
    [Header("General Settings")]
    public bool Trigger;
    public NucleonBody Body;
    public NucleonMaterial NucleonMaterial;

    [Header("Collider Customization")]
    public Vector3 Position;
    public Vector3 Scale;

    [Header("Specific Settings")]
    public float CollisionOverlapThreshold = 0.001f;

    [Header("Debug Settings")]
    public bool DebugCollisionPoints;
    public bool DebugCenterOfGravity;
    
    public CubeModel CubeModel { get; private set; }
    public bool Colliding { get; private set; }
    public List<NucleonCollider> ActiveCollisions { get; private set; }
    public List<Vector3> CollidingPoints { get; private set; }
    public Vector3 DeltaPosition { get; private set; }
    public Vector3 LastPosition { get; private set; }

    private NucleonManager NucleonManager;

    void Awake()
    {
        CubeModel = new CubeModel();
        CubeModel.UpdateBounds(transform, Position, Scale);

        ActiveCollisions = new List<NucleonCollider>();
        CollidingPoints = new List<Vector3>();

        NucleonManager = NucleonManager.Instance;
        NucleonManager.RegisterCollider(this);
    }

    void Update()
    {
        CubeModel.UpdateBounds(transform, Position, Scale);
        Colliding = ActiveCollisions.Count > 0;
        DeltaPosition = (transform.position + Position) - LastPosition;
        LastPosition = (transform.position + Position);

        FetchCollisions();
    }

    public void FetchCollisions()
    {
        foreach (NucleonBoxCollider BoxCollider in NucleonManager.Colliders)
        {
            if(BoxCollider != this)
            {
                bool Colliding = NucleonIntersector.CC_I(CubeModel, BoxCollider.CubeModel);
                CollisionCheck(Colliding, BoxCollider);
                
                if (Colliding && DebugCollisionPoints)
                {
                    List<Vector3> CollidingPointsFetchList = new List<Vector3>();
                    foreach (Vector3 Vertice in CubeModel.Vertices)
                    {
                        if (NucleonIntersector.PC_I(Vertice, BoxCollider.CubeModel))
                        {
                            CollidingPointsFetchList.Add(Vertice);
                        }
                    }
                    CollidingPoints = CollidingPointsFetchList;
                }
                if (!Colliding && DebugCollisionPoints)
                {
                    CollidingPoints = new List<Vector3>();
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
            
            if (DebugCollisionPoints)
            {
                Gizmos.color = Color.blue;
                foreach (Vector3 Point in CollidingPoints)
                {
                    Gizmos.DrawSphere(Point, 0.1f);
                }
            }

            if (DebugCenterOfGravity)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(transform.position, 0.1f);
            }
        }
        catch{}
    }
}