using NucleonEngine.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonBody : MonoBehaviour
{
    [Header("Physical Settings")]
    public float Mass;
    public float GravityScale;

    [HideInInspector] public Vector3 Velocity;

    public Vector3 AbsoluteVelocity;
    public bool Grounded { get; private set; }
    public NucleonBoxCollider Ground { get; private set; }

    private Vector3 CalculationVelocity;
    private float GravityTimer;

    private Dictionary<int, bool[]> TouchingFaces = new Dictionary<int, bool[]>();

    void Start()
    {
        Grounded = false;
    }

    void Update()
    {
        GravityTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!Grounded)
        {
            CalculationVelocity.y += -GravityScale * GravityTimer;
        }

        PrepareVelocity();
        transform.position += AbsoluteVelocity * Time.fixedDeltaTime;
    }

    public void OnNucleonCollisionEnter(NucleonCollision Collision)
    {
        if (!TouchingFaces.ContainsKey(Collision.OtherCollider.GetInstanceID()))
        {
            TouchingFaces.Add(Collision.OtherCollider.GetInstanceID(), new bool[0]);
        }
        bool[] FacesArray = {Collision.TouchingMinX, Collision.TouchingMaxX, Collision.TouchingMinY, Collision.TouchingMaxY, Collision.TouchingMinZ, Collision.TouchingMaxZ};
        TouchingFaces[Collision.OtherCollider.GetInstanceID()] = FacesArray;

        if (Collision.CollisionDirection.y < 0 && !FacesArray[0] && !FacesArray[1] && !FacesArray[4] && !FacesArray[5])
        {
            Grounded = true;
            Ground = Collision.OtherCollider;
            CalculationVelocity.y = 0;
            GravityTimer = 0;
            transform.position = new Vector3(
                transform.position.x + Collision.SelfCollider.Position.x, 
                ((transform.localScale.y + Collision.SelfCollider.Scale.y) / 2) + Collision.OtherCollider.CubeModel.MaxY - Collision.SelfCollider.CollisionOverlapThreshold - Collision.SelfCollider.Position.y, 
                transform.position.z + Collision.SelfCollider.Position.z);
        }
    }

    public void OnNucleonCollisionExit(NucleonCollisionExit CollisionExit)
    {
        TouchingFaces.Remove(CollisionExit.OtherCollider.GetInstanceID());

        if (CollisionExit.OtherCollider == Ground)
        {
            Grounded = false;
            Ground = null;
            GravityTimer = 0;
        }
    }

    public void AddForce(float Force, Vector3 Direction)
    {
        float Acceleration = Force / Mass;
        CalculationVelocity += new Vector3(Direction.x * Acceleration, Direction.y * Acceleration, Direction.z * Acceleration);
    }

    private void PrepareVelocity()
    {
        AbsoluteVelocity = CalculationVelocity + Velocity;

        foreach(var CollisionSide in TouchingFaces)
        {
            if(CollisionSide.Value[0] == true && AbsoluteVelocity.x > 0)
            {
                AbsoluteVelocity.x = 0;
            }
            if (CollisionSide.Value[1] == true && AbsoluteVelocity.x < 0)
            {
                AbsoluteVelocity.x = 0;
            }

            if (CollisionSide.Value[2] == true && AbsoluteVelocity.y > 0)
            {
                AbsoluteVelocity.y = 0;
            }

            if (CollisionSide.Value[4] == true && AbsoluteVelocity.z > 0)
            {
                AbsoluteVelocity.z = 0;
            }
            if (CollisionSide.Value[5] == true && AbsoluteVelocity.z < 0)
            {
                AbsoluteVelocity.z = 0;
            }
        }
    }
}