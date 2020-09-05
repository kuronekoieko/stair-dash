using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    void Start()
    {

    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * 10f;
    }

    void Update()
    {

    }
}
