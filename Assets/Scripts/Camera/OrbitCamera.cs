using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _rotSpeed = 1.5f;
    private float _rotY;
    private Vector3 _offset;


    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = _target.position - transform.position;
    }

   
    void LateUpdate()
    {
        _rotY += Input.GetAxis("Mouse X") * _rotSpeed;

        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = _target.position - (rotation * _offset);
        transform.LookAt(_target);

    }
}
