using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _rotSpeed = 1.5f;
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

   
    void LateUpdate()
    {
        _rotY += Input.GetAxis("Mouse X") * _rotSpeed;
        _rotX -= Input.GetAxis("Mouse Y") * _rotSpeed;

        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
        transform.position = _target.position - (rotation * _offset);
        transform.LookAt(_target);

    }
}
