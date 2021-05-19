using NucleonEngine.Calculations;
using NucleonEngine.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Collections
{
    public class NucleonCollision
    {
        public NucleonBoxCollider SelfCollider;
        public NucleonBoxCollider OtherCollider;
        public svector3 CollisionDirection;
        public sfloat ImpactForce;

        public bool TouchingMinX;
        public bool TouchingMaxX;
        public bool TouchingMinY;
        public bool TouchingMaxY;
        public bool TouchingMinZ;
        public bool TouchingMaxZ;
    }
}