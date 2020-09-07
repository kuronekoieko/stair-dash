using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Sitting = 0,
    WalkWithLuggages = 1,
}
public class RagDollController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem hitPS;
    [SerializeField] ObstacleType obstacleType;
    [SerializeField] Rigidbody parentRb;
    [SerializeField] CapsuleCollider parentCollider;
    [SerializeField] Transform luggageParentTf;
    Rigidbody[] ragdollRbs;
    Rigidbody[] luggageRbs;
    private void Awake()
    {
        ragdollRbs = animator.transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
        luggageRbs = luggageParentTf.GetComponentsInChildren<Rigidbody>();
    }
    void Start()
    {
        parentRb.isKinematic = false;
        switch (obstacleType)
        {
            case ObstacleType.Sitting:
                parentRb.isKinematic = true;
                break;
            case ObstacleType.WalkWithLuggages:
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (Variables.screenState != ScreenState.Game) return;
        switch (obstacleType)
        {
            case ObstacleType.Sitting:

                break;
            case ObstacleType.WalkWithLuggages:
                var vel = parentRb.velocity;
                vel.z = transform.forward.z * 2f;
                parentRb.velocity = vel;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HitPlayer(other);
    }

    void HitPlayer(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }
        parentCollider.enabled = false;
        //hitPS.transform.position = other.ClosestPoint(other.transform.position);
        hitPS.Play();
        parentRb.isKinematic = true;
        Variables.hitCount++;
        foreach (var rb in ragdollRbs)
        {
            rb.isKinematic = false;
            var vec = transform.position - other.transform.position;
            rb.AddForce(vec.normalized * 100f, ForceMode.Impulse);
            animator.enabled = false;
        }

        foreach (var rb in luggageRbs)
        {
            rb.isKinematic = false;
            var randomAngle = Random.Range(0, 360);
            var angles = new Vector3(0, randomAngle, 0);
            var direction = Quaternion.Euler(angles) * Vector3.forward;
            rb.AddForce(direction.normalized * 5f, ForceMode.Impulse);
        }
    }
}
