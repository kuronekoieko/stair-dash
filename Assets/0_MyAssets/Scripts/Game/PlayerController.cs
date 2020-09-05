using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody forwardRb;
    [SerializeField] Rigidbody horizontalRb;
    Vector3 tapPos;
    float horizontalSpeed;
    float horizontalSpeedLimit = 20f;
    float forwardSpeed = 20f;

    bool isCurving;
    Vector3 curvePos;
    float curveRadius;

    void Start()
    {

    }

    private void FixedUpdate()
    {

        if (isCurving)
        {
            forwardRb.velocity = Vector3.zero;
            transform.RotateAround(curvePos, Vector3.up, -forwardSpeed / curveRadius);
        }
        else
        {
            var forward = Vector3.zero;
            forward.z = transform.forward.z;
            transform.forward = forward;
            forwardRb.velocity = transform.forward * forwardSpeed;
        }

        horizontalRb.velocity = horizontalRb.transform.right * horizontalSpeed;

    }

    private void LateUpdate()
    {
        var localPos = horizontalRb.transform.localPosition;
        localPos.y = 0;
        localPos.z = 0;
        horizontalRb.transform.localPosition = localPos;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tapPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 drag = Input.mousePosition - tapPos;
            tapPos = Input.mousePosition;
            SetHorizontalVel(xSpeed: drag.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetHorizontalVel(xSpeed: 0);
        }
    }

    void SetHorizontalVel(float xSpeed)
    {
        horizontalSpeed = Mathf.Clamp(xSpeed, -horizontalSpeedLimit, horizontalSpeedLimit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CurvePoint"))
        {
            isCurving = !isCurving;
            curvePos = other.transform.position;
            curvePos.y = transform.position.y;
            curveRadius = Vector3.Distance(curvePos, transform.position);
        }
    }

    Transform GetCurvePoint()
    {
        Ray ray = new Ray(transform.position, -transform.right);
        float distance = 10f;
        Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, distance);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);
        if (hit.collider == null) return null;
        if (!hit.collider.gameObject.CompareTag("CurvePoint")) return null;
        return hit.collider.transform;
    }
}
