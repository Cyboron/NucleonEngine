using System.Numerics;

namespace NucleonEngine.Calculations
{
    public class svector3
    {
        public sfloat x;
        public sfloat y;
        public sfloat z;
        private sfloat hash;

        public svector3(sfloat x, sfloat y, sfloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            hash = (sfloat) 0;
        }

        public Vector3 ToUnityVector()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }

        public static svector3 Zero()
        {
            return new svector3((sfloat) 0,(sfloat) 0,(sfloat) 0);
        }

        public static svector3 One()
        {
            return new svector3((sfloat) 1, (sfloat) 1, (sfloat) 1);
        }

        public static svector3 operator +(svector3 a, svector3 b)
        {
            a.x += b.x;
            a.y += b.y;
            a.z += b.z;

            return a;
        }

        public static svector3 operator -(svector3 a, svector3 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;

            return a;
        }

        public static svector3 operator *(svector3 a, svector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;

            return a;
        }

        public static svector3 operator /(svector3 a, svector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;

            return a;
        }

        public static bool operator ==(svector3 a, svector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(svector3 a, svector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public override bool Equals(object obj)
        {
            if(obj is svector3 vector3)
            {
                return x == vector3.x && y == vector3.y && z == vector3.z;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return hash.GetHashCode();
        }
    }
}