using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody forwardRb;
    [SerializeField] Rigidbody horizontalRb;
    [SerializeField] Collider preGoalCollider;
    [SerializeField] Collider goalCollider;
    Vector3 tapPos;
    float horizontalSpeed;
    readonly float horizontalLimit = 2.5f;
    readonly float forwardSpeed = 20f;

    Vector3 horizontalLocalPos;
    bool isCurving;
    Vector3 curvePos;
    float curveRadius;
    float horizontalInterpolate;
    readonly float horizontalDragMax = 200;
    bool isPreGoaled;

    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (Variables.screenState == ScreenState.Start) return;
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
    }

    void Update()
    {

        if (isPreGoaled)
        {
            var pos = Vector3.zero;
            pos.x = horizontalRb.transform.localPosition.x;
            horizontalRb.transform.localPosition = pos;
            return;
        }
        if (Variables.screenState != ScreenState.Game) return;
        if (Input.GetMouseButtonDown(0))
        {
            var rate = InverseLerpUnclamped(0, horizontalLimit, horizontalRb.transform.localPosition.x);
            horizontalInterpolate = Mathf.Clamp(rate, -1, 1);

            tapPos = Input.mousePosition;
            tapPos.x = GetLerpMin(tapPos.x, horizontalDragMax, horizontalInterpolate);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 drag = Input.mousePosition - tapPos;
            horizontalInterpolate = Mathf.Clamp(drag.x / horizontalDragMax, -1, 1);
        }

        horizontalLocalPos = Vector3.LerpUnclamped(Vector3.zero, Vector3.right * horizontalLimit, horizontalInterpolate);
        horizontalRb.transform.localPosition = horizontalLocalPos;
    }

    private void LateUpdate()
    {
        //horizontalRb.transform.localPosition = horizontalLocalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        Curve(other);
        PreGoal(other);
        Goal(other);
    }

    void Goal(Collider other)
    {
        if (other != goalCollider) return;
        if (Variables.screenState != ScreenState.Game) return;
        Variables.screenState = ScreenState.Clear;
    }

    void PreGoal(Collider other)
    {
        if (other != preGoalCollider) return;
        isPreGoaled = true;
        horizontalRb.transform.DOLocalMoveX(0, 0.3f);
    }

    void Curve(Collider other)
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


    float InverseLerpUnclamped(float min, float max, float value)
    {
        return value / (max - min);
    }


    /*
    lerp = (max - min) * t + min ・・・ ①
    max = min + 100 ・・・②
    ①、②より
    min = lerp -100 * t
    */
    float GetLerpMin(float lerp, float maxDistance, float t)
    {
        return lerp - maxDistance * t;
    }
}
