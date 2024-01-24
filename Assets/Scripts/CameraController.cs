using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5;

    [SerializeField] float minVerticalAngle = - 10;
    [SerializeField] float maxVerticalAngle =  45;

    [SerializeField] Vector2 framingOffSet;

    float rotationX;
    float rotationY;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float invertXVal;
    float invertYVal;
    private void Update()
    {
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Cursor.lockState = CursorLockMode.Locked;

                invertXVal = (invertX) ? -1 : 1;
                invertYVal = (invertY) ? -1 : 1;

                rotationX -= Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
                rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
                rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
            var focusPosition = followTarget.position + new Vector3(framingOffSet.x, framingOffSet.y);

            transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
            transform.rotation = targetRotation;
        }
    }

            public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
    

}
