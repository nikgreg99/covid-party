using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _sensitivity = 1.5f;
    [SerializeField] private float _maxAngleRotationX = 90.0f;
    [SerializeField] private float _maxAngleRotationY = 360.0f;

    private float _rotY;
    private float _rotX;
    private Vector3 _offset;


    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = _target.position - transform.position;

#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }


    void Update()
    {
        _rotY += Input.GetAxis("Mouse X") * _sensitivity;
        _rotX -= Input.GetAxis("Mouse Y") * _sensitivity;

        _rotX = Mathf.Clamp(_rotX, -_maxAngleRotationX, _maxAngleRotationX);
        _rotY = Mathf.Clamp(_rotY, -_maxAngleRotationY, _maxAngleRotationY);

        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = _target.position - (rotation * _offset);
        transform.LookAt(_target);

    }
}
