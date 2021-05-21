using NucleonEngine.Calculations;

namespace NucleonEngine.Collisions
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