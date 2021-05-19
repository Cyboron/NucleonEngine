using NucleonEngine.Calculations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Collections
{
    public class NucleonCollisionExit
    {
        public NucleonBoxCollider SelfCollider;
        public NucleonBoxCollider OtherCollider;
        public svector3 ExitDirection;
    }
}