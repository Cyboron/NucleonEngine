using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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