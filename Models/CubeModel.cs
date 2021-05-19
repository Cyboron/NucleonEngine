using NucleonEngine.Calculations;
using UnityEngine;

namespace NucleonEngine.Models
{
    public class CubeModel
    {
        public sfloat MaxX;
        public sfloat MinX;
        public sfloat MaxY;
        public sfloat MinY;
        public sfloat MaxZ;
        public sfloat MinZ;
        public svector3[] Vertices;
        public svector3 Position;
        public svector3 Scale;

        private Transform transform;

        public void UpdateBounds(Transform transform, svector3 Position, svector3 Scale)
        {
            this.transform = transform;
            this.Position = Position;
            this.Scale = Scale;

            MaxX = ((sfloat)transform.position.x + Position.x + ((sfloat)transform.localScale.x + Scale.x) / (sfloat)2);
            MinX = ((sfloat)transform.position.x + Position.x + ((sfloat)transform.localScale.x + Scale.x) / -(sfloat)2);
            MaxY = ((sfloat)transform.position.y + Position.y + ((sfloat)transform.localScale.y + Scale.y) / (sfloat)2);
            MinY = ((sfloat)transform.position.y + Position.y + ((sfloat)transform.localScale.y + Scale.y) / -(sfloat)2);
            MaxZ = ((sfloat)transform.position.z + Position.z + ((sfloat)transform.localScale.z + Scale.z) / (sfloat)2);
            MinZ = ((sfloat)transform.position.z + Position.z + ((sfloat)transform.localScale.z + Scale.z) / -(sfloat)2);

            Vertices = new[]
            {
            new svector3(MaxX, MaxY, MaxZ),
            new svector3(MinX, MinY, MinZ),

            new svector3(MaxX, MinY, MinZ),
            new svector3(MinX, MaxY, MinZ),
            new svector3(MinX, MinY, MaxZ),

            new svector3(MaxX, MinY, MaxZ),
            new svector3(MinX, MaxY, MaxZ),
            new svector3(MaxX, MaxY, MinZ),
        };
        }
    }
}