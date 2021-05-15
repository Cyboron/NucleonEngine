using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonBoxCollider : MonoBehaviour, NucleonCollider
{
    public bool Trigger;
    public NucleonMaterial NucleonMaterial;
    public float CollisionOverlapThreshold = 0.001f;
    public Vector3 Position;
    public Vector3 Scale;

    [HideInInspector] public CubeModel CubeModel { get; private set; }
    [HideInInspector] public bool Colliding { get; private set; }
    [HideInInspector] public List<NucleonCollider> ActiveCollisions { get; private set; }
    [HideInInspector] public Vector3 DeltaPosition;
    [HideInInspector] public Vector3 LastPosition;

    private NucleonManager NucleonManager;
    private List<Vector3> CollidingPoints = new List<Vector3>();

    void Awake()
    {
        CubeModel = new CubeModel();
        CubeModel.UpdateBounds(transform, Position, Scale);

        ActiveCollisions = new List<NucleonCollider>();
        NucleonManager = FindObjectOfType<NucleonManager>();

        InvokeRepeating("UpdateCollisions", 0.0f, 0.01f);
    }

    void UpdateCollisions()
    {
        CubeModel.UpdateBounds(transform, Position, Scale);
        Colliding = ActiveCollisions.Count > 0;
        DeltaPosition = (transform.position + Position) - LastPosition;
        LastPosition = (transform.position + Position);

        FetchCollisions();
    }

    public void FetchCollisions()
    {
        List<Vector3> CollidingPointsFetchList = new List<Vector3>();

        foreach (NucleonBoxCollider BoxCollider in NucleonManager.Colliders)
        {
            if(BoxCollider != this)
            {
                bool Colliding = NucleonIntersector.CC_I(CubeModel, BoxCollider.CubeModel);
                CollisionCheck(Colliding, BoxCollider);

                if (Colliding)
                {
                    foreach (Vector3 Verticie in CubeModel.Vertices)
                    {
                        if(NucleonIntersector.PC_I(Verticie, BoxCollider.CubeModel))
                        {
                            CollidingPointsFetchList.Add(Verticie);
                        }
                    }
                }
            }
        }

        CollidingPoints = CollidingPointsFetchList;
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
                GetComponent<NucleonBody>()?.OnNucleonCollisionEnter(Collision);
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
                GetComponent<NucleonBody>()?.OnNucleonCollisionExit(CollisionExit);
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

            Gizmos.color = Color.blue;
            foreach (Vector3 Point in CollidingPoints)
            {
                Gizmos.DrawSphere(Point, 0.1f);
            }
        }
        catch{}
    }
}