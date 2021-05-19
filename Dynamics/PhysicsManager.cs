using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Dynamics
{
    public class PhysicsManager : MonoBehaviour
    {
        public static PhysicsManager Instance;
        public List<Collisions.Collider> Colliders = new List<Collisions.Collider>();

        void Awake()
        {
            Instance = this;
        }

        public void RegisterCollider(Collisions.Collider Collider)
        {
            if (!Colliders.Contains(Collider))
            {
                Colliders.Add(Collider);
            }
        }

        public void RemoveCollider(Collisions.Collider Collider)
        {
            if (Colliders.Contains(Collider))
            {
                Colliders.Remove(Collider);
            }
        }
    }
}