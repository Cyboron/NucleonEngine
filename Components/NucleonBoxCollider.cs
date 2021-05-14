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
        foreach (NucleonBoxCollider BoxCollider in NucleonManager.Colliders)
        {
            if(BoxCollider != this)
            {
                bool Colliding = CubeIntersect(CubeModel, BoxCollider.CubeModel);
                CollisionCheck(Colliding, BoxCollider);
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

    private bool CubeIntersect(CubeModel A, CubeModel B)
    {
        return (A.MinX <= B.MaxX && A.MaxX >= B.MinX) &&
               (A.MinY <= B.MaxY && A.MaxY >= B.MinY) &&
               (A.MinZ <= B.MaxZ && A.MaxZ >= B.MinZ);
    }

    private bool PointCubeIntersect(Vector3 Point, CubeModel Cube)
    {
        return (Point.x >= Cube.MinX && Point.x <= Cube.MaxX) &&
               (Point.y >= Cube.MinY && Point.y <= Cube.MaxY) &&
               (Point.z >= Cube.MinZ && Point.z <= Cube.MaxZ);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        try
        {
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
        }
        catch{}
    }
}

public class CubeModel
{
    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;
    public float MaxZ;
    public float MinZ;
    public Vector3[] Vertices;
    public Vector3 Position;
    public Vector3 Scale;

    private Transform transform;

    public void UpdateBounds(Transform transform, Vector3 Position, Vector3 Scale)
    {
        this.transform = transform;
        this.Position = Position;
        this.Scale = Scale;

        MaxX = (transform.position.x + Position.x + (transform.localScale.x + Scale.x) / 2);
        MinX = (transform.position.x + Position.x + (transform.localScale.x + Scale.x) / -2);
        MaxY = (transform.position.y + Position.y + (transform.localScale.y + Scale.y) / 2);
        MinY = (transform.position.y + Position.y + (transform.localScale.y + Scale.y) / -2);
        MaxZ = (transform.position.z + Position.z + (transform.localScale.z + Scale.z) / 2);
        MinZ = (transform.position.z + Position.z + (transform.localScale.z + Scale.z) / -2);

        Vertices = new[]
        { 
            new Vector3(MaxX, MaxY, MaxZ),
            new Vector3(MinX, MinY, MinZ),

            new Vector3(MaxX, MinY, MinZ),
            new Vector3(MinX, MaxY, MinZ),
            new Vector3(MinX, MinY, MaxZ),

            new Vector3(MaxX, MinY, MaxZ),
            new Vector3(MinX, MaxY, MaxZ),
            new Vector3(MaxX, MaxY, MinZ),
        };
    }
}