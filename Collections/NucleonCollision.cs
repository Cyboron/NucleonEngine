using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonCollision
{
    public NucleonBoxCollider SelfCollider;
    public NucleonBoxCollider OtherCollider;
    public Vector3 CollisionDirection;
    public float ImpactForce;
}