using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    PlayerController playerController;
    Vector3 offset;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        offset = transform.position - playerController.transform.position;
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.position = playerController.transform.position + offset;
    }
}
