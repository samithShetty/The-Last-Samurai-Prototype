using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//let camera follow target
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // Player (or whatever object camera is following)
    [SerializeField] private Transform cameraHolderTransform; // For the Camera Grand-Parent Object
    [SerializeField] private Transform cameraPivotTransform; // For the Camera Parent Object
    [SerializeField] private float cameraFollowTime; // Lower -> Faster Camera Following
    [SerializeField] private float cameraLookSpeed;
    [SerializeField] private float minPivotAngle;
    [SerializeField] private float maxPivotAngle;
    
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector2 look;
    private float horizontalPivotAngle;
    private float verticalPivotAngle;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        RotateCamera();
        FollowTarget();
    }

    private void FollowTarget()
    {
        cameraHolderTransform.position = Vector3.SmoothDamp(cameraHolderTransform.position, targetTransform.position, ref cameraVelocity, cameraFollowTime); 
    }

    private void RotateCamera()
    {
        horizontalPivotAngle += look.x * cameraLookSpeed;
        verticalPivotAngle -= look.y * cameraLookSpeed;
        verticalPivotAngle = Mathf.Clamp(verticalPivotAngle, minPivotAngle, maxPivotAngle);

        cameraHolderTransform.rotation = Quaternion.Euler(new Vector3(0,horizontalPivotAngle,0)); // y rotation = left/right
        cameraPivotTransform.localRotation = Quaternion.Euler(new Vector3(verticalPivotAngle,0,0)); // X rotation = up/down
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public float GetAngle()
    {
        return horizontalPivotAngle;
    }
}

