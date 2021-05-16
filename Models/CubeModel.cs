using UnityEngine;

namespace NucleonEngine.Models
{
    public class CubeModel
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;
        public float MaxZ;
        public float MinZ;
        public Vector3[] Vertices;
        public Vector3 Position;
        public Vector3 Scale;

        private Transform transform;

        public void UpdateBounds(Transform transform, Vector3 Position, Vector3 Scale)
        {
            this.transform = transform;
            this.Position = Position;
            this.Scale = Scale;

            MaxX = (transform.position.x + Position.x + (transform.localScale.x + Scale.x) / 2);
            MinX = (transform.position.x + Position.x + (transform.localScale.x + Scale.x) / -2);
            MaxY = (transform.position.y + Position.y + (transform.localScale.y + Scale.y) / 2);
            MinY = (transform.position.y + Position.y + (transform.localScale.y + Scale.y) / -2);
            MaxZ = (transform.position.z + Position.z + (transform.localScale.z + Scale.z) / 2);
            MinZ = (transform.position.z + Position.z + (transform.localScale.z + Scale.z) / -2);

            Vertices = new[]
            {
            new Vector3(MaxX, MaxY, MaxZ),
            new Vector3(MinX, MinY, MinZ),

            new Vector3(MaxX, MinY, MinZ),
            new Vector3(MinX, MaxY, MinZ),
            new Vector3(MinX, MinY, MaxZ),

            new Vector3(MaxX, MinY, MaxZ),
            new Vector3(MinX, MaxY, MaxZ),
            new Vector3(MaxX, MaxY, MinZ),
        };
        }
    }
}