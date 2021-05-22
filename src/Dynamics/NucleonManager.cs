using NucleonEngine.Collisions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NucleonEngine.Dynamics
{
    public class NucleonManager : MonoBehaviour
    {
        public static NucleonManager Instance;
        // public List<NucleonCollider> Colliders = new List<NucleonCollider>();
        public NucleonCollider[] Colliders;

        void Awake()
        {
            Instance = this;

            Colliders = FindObjectsOfType<MonoBehaviour>().OfType<NucleonCollider>().ToArray();
        }

        /* public void RegisterCollider(NucleonCollider Collider)
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
        } */
    }
}