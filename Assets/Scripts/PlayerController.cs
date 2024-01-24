using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CameraController cameraController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    [SerializeField] float moveSpeed = 5f;
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var moveInput = (new Vector3 (horizontal, 0, vertical)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
