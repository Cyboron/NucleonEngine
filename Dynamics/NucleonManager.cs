using NucleonEngine.Collisions;
using System.Collections.Generic;
using UnityEngine;

namespace NucleonEngine.Dynamics
{
    public class NucleonManager : MonoBehaviour
    {
        public static NucleonManager Instance;
        public List<NucleonCollider> Colliders = new List<NucleonCollider>();

        void Awake()
        {
            Instance = this;
        }

        public void RegisterCollider(NucleonCollider Collider)
        {
            if (!Colliders.Contains(Collider))
            {
                Colliders.Add(Collider);
            }
        }

        public void RemoveCollider(NucleonCollider Collider)
        {
            if (Colliders.Contains(Collider))
            {
                Colliders.Remove(Collider);
            }
        }
    }
}