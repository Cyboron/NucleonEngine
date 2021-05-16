using NucleonEngine.Models;
using UnityEngine;

namespace NucleonEngine.Collections
{
    public static class NucleonIntersector
    {
        /*
         * P = Point
         * C = Cube
         * I = Intersect
        */

        public static bool PC_I(Vector3 Point, CubeModel Cube)
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