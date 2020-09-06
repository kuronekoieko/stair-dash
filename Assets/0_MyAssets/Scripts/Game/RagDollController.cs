using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{
    [SerializeField] Animator animator;
    Rigidbody[] ragdollRbs;
    private void Awake()
    {
        ragdollRbs = GetComponentsInChildren<Rigidbody>();
    }
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var rb in ragdollRbs)
            {
                var vec = transform.position - other.transform.position;
                rb.AddForce(vec.normalized * 100f, ForceMode.Impulse);
                animator.enabled = false;
            }
        }
    }
}
