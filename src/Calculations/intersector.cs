using NucleonEngine.Collisions;

namespace NucleonEngine.Calculations
{
    public static class intersector
    {
        /*
         * P = Point
         * C = Cube
         * I = Intersect
        */

        // Main Collision Checks

        public static bool PC_I(svector3 Point, CubeModel Cube)
        {
            return (Point.x >= Cube.MinX && Point.x <= Cube.MaxX) &&
                   (Point.y >= Cube.MinY && Point.y <= Cube.MaxY) &&
                   (Point.z >= Cube.MinZ && Point.z <= Cube.MaxZ);
        }

        public static bool CC_I(CubeModel A, CubeModel B)
        {
            return (A.MinX <= B.MaxX && A.MaxX >= B.MinX) &&
                   (A.MinY <= B.MaxY && A.MaxY >= B.MinY) &&
                   (A.MinZ <= B.MaxZ && A.MaxZ >= B.MinZ);
        }
    }
}