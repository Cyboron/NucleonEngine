using NucleonEngine.Calculations;

namespace NucleonEngine.Collisions
{
    public class Collision
    {
        public BoxCollider SelfCollider;
        public BoxCollider OtherCollider;
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