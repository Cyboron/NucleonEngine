using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonBody : MonoBehaviour
{
    [Header("Physical Settings")]
    public float Mass;
    public float GravityScale;

    [HideInInspector] public Vector3 Velocity;

    public Vector3 AbsoluteVelocity { get; private set; }
    public bool Grounded { get; private set; }
    public NucleonBoxCollider Ground { get; private set; }

    private Vector3 CalculationVelocity;
    private float GravityTimer;

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

        AbsoluteVelocity = CalculationVelocity + Velocity;
        transform.position += AbsoluteVelocity * Time.fixedDeltaTime;
    }

    public void OnNucleonCollisionEnter(NucleonCollision Collision)
    {
        if (Collision.CollisionDirection.y < 0)
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
        if (Collision.CollisionDirection.y > 0)
        {
            CalculationVelocity.y = 0;
        }
    }

    public void OnNucleonCollisionExit(NucleonCollisionExit CollisionExit)
    {
        if(CollisionExit.OtherCollider == Ground)
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
}