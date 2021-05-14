using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonBody : MonoBehaviour
{
    public float Mass;
    public float GravityScale;

    [HideInInspector] public Vector3 Velocity;
    [HideInInspector] public bool Grounded { get; private set; }
    [HideInInspector] public NucleonBoxCollider Ground { get; private set; }

    private float GravityTimer;

    void Start()
    {
        Grounded = false;
    }

    void Update()
    {
        transform.position += Velocity * Time.deltaTime;
        GravityTimer += Time.deltaTime;

        if (!Grounded)
        {
            Velocity.y += -GravityScale / 100 * GravityTimer;
        }
    }

    public void OnNucleonCollisionEnter(NucleonCollision Collision)
    {
        if(Collision.CollisionDirection.y < 0)
        {
            Grounded = true;
            Ground = Collision.OtherCollider;
            Velocity.y = 0;
            GravityTimer = 0;
            transform.position = new Vector3(transform.position.x, (transform.localScale.y / 2) + Collision.OtherCollider.CubeModel.MaxY - Collision.SelfCollider.CollisionOverlapThreshold, transform.position.z);
        }
    }

    public void OnNucleonCollisionExit()
    {
        Grounded = false;
        Ground = null;
        GravityTimer = 0;
    }

    public void AddForce(float Force, Vector3 Direction)
    {
        float Acceleration = Force / Mass;
        Velocity += new Vector3(Direction.x * Acceleration, Direction.y * Acceleration, Direction.z * Acceleration);
        GravityTimer = 0;
    }
}