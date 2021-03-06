using UnityEngine;

namespace NucleonEngine.Calculations
{
    public class svector3
    {
        public sfloat x = (sfloat)0;
        public sfloat y = (sfloat)0;
        public sfloat z = (sfloat)0;
        private sfloat hash = (sfloat)0;

        public svector3(sfloat x, sfloat y, sfloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public svector3(int x, int y, int z)
        {
            this.x = (sfloat)x;
            this.y = (sfloat)y;
            this.z = (sfloat)z;
        }

        public static svector3 Zero()
        {
            return new svector3((sfloat)0,(sfloat)0,(sfloat)0);
        }

        public static svector3 One()
        {
            return new svector3((sfloat)1, (sfloat)1, (sfloat)1);
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

        public static svector3 operator *(svector3 a, sfloat b)
        {
            a.x *= b;
            a.y *= b;
            a.z *= b;

            return a;
        }

        public static svector3 operator /(svector3 a, sfloat b)
        {
            a.x /= b;
            a.y /= b;
            a.z /= b;

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

        public static explicit operator svector3(Vector3 vector)
        {
            return new svector3((sfloat)vector.x, (sfloat)vector.y, (sfloat)vector.z);
        }

        public static explicit operator Vector3(svector3 vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
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