using NucleonEngine.Calculations;
using NucleonEngine.Dynamics;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float Speed = 5;
    public float JumpForce = 6;

    private NucleonRigidbody Body;

    void Awake()
    {
        Body = GetComponent<NucleonRigidbody>();
    }

    void Update()
    {
        Body.Velocity = new svector3((sfloat)Input.GetAxis("Horizontal") * (sfloat)Speed, (sfloat)0, (sfloat)Input.GetAxis("Vertical") * (sfloat)Speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Body.Grounded)
            {
                Body.AddForce((sfloat)JumpForce, new svector3(0, 1, 0));
            }
        }
    }
}