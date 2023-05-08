using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotationSpeed;
    private Transform camTransform;
    private Vector3 pivotPoint;

    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        pivotPoint = camTransform.position;
        camTransform.position = pivotPoint+offset;
    }

    // Update is called once per frame
    void Update()
    {
        camTransform.RotateAround(pivotPoint, Vector3.up, Time.deltaTime*rotationSpeed);
        camTransform.LookAt(pivotPoint);
    }
}
