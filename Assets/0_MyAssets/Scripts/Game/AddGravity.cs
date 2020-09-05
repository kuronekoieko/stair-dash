using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGravity : MonoBehaviour
{
    [SerializeField] float downForce;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        rb.AddForce(Vector3.down * downForce, ForceMode.Acceleration);
    }
}
