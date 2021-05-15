using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NucleonMaterial", menuName = "Nucleon/Material", order = 1)]
public class NucleonMaterial : ScriptableObject
{
    public float Bounciness;
    public bool Sticky;
}
