using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Collections
{
    public class NucleonCollision
    {
        public NucleonBoxCollider SelfCollider;
        public NucleonBoxCollider OtherCollider;
        public Vector3 CollisionDirection;
        public float ImpactForce;
    }
}