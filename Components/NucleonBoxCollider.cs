using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonBoxCollider : MonoBehaviour, NucleonCollider
{
    public bool Trigger;
    public NucleonMaterial NucleonMaterial;
    public float CollisionOverlapThreshold = 0.001f;

    [HideInInspector] public CubeModel CubeModel { get; private set; }
    [HideInInspector] public bool Colliding { get; private set; }
    [HideInInspector] public List<NucleonCollider> ActiveCollisions { get; private set; }
    [HideInInspector] public Vector3 PositionDelta;
    [HideInInspector] public Vector3 LastPosition;

    private Transform TransformInstance;
    private NucleonManager NucleonManager;

    void Awake()
    {
        TransformInstance = GetComponent<Transform>();

        CubeModel = new CubeModel(TransformInstance);
        CubeModel.UpdateBounds();

        ActiveCollisions = new List<NucleonCollider>();
        NucleonManager = FindObjectOfType<NucleonManager>();

        InvokeRepeating("UpdateCollisions", 0.0f, 0.01f);
    }

    void UpdateCollisions()
    {
        CubeModel.UpdateBounds();
        Colliding = ActiveCollisions.Count > 0;
        PositionDelta = transform.position - LastPosition;
        LastPosition = transform.position;

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
            NucleonCollision collision = new NucleonCollision();
            collision.SelfCollider = this;
            collision.OtherCollider = BoxCollider;
            collision.CollisionDirection = PositionDelta;

            if (!ActiveCollisions.Contains(BoxCollider))
            {
                ActiveCollisions.Add(BoxCollider);
                GetComponent<NucleonBody>()?.OnNucleonCollisionEnter(collision);
            }
        }
        else
        {
            if (ActiveCollisions.Contains(BoxCollider))
            {
                ActiveCollisions.Remove(BoxCollider);
                GetComponent<NucleonBody>()?.OnNucleonCollisionExit();
            }
        }
    }

    private bool CubeIntersect(CubeModel A, CubeModel B)
    {
        return (A.MinX <= B.MaxX && A.MaxX >= B.MinX) &&
               (A.MinY <= B.MaxY && A.MaxY >= B.MinY) &&
               (A.MinZ <= B.MaxZ && A.MaxZ >= B.MinZ);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        try
        {
            foreach (Vector3 Verticie in CubeModel.Vertices)
            {
                Gizmos.DrawSphere(Verticie, 0.05f);
            }
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

    private Transform transform;

    public CubeModel(Transform transformInstance)
    {
        transform = transformInstance;
    }

    public void UpdateBounds()
    {
        MaxX = transform.position.x + transform.localScale.x / 2;
        MinX = transform.position.x + transform.localScale.x / -2;
        MaxY = transform.position.y + transform.localScale.y / 2;
        MinY = transform.position.y + transform.localScale.y / -2;
        MaxZ = transform.position.z + transform.localScale.z / 2;
        MinZ = transform.position.z + transform.localScale.z / -2;

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