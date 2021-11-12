using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotXSpeed = 1.5f;
    [SerializeField] private float _rotYSpeed = 1.5f;
    [SerializeField] private float _rotYMaxAngle = 360.0f;

    private float _rotX;
    private float _rotY;

    // Start is called before the first frame update
    void Start()
    {
        _rotX = _target.eulerAngles.x;
        _rotY = _target.eulerAngles.y;
    }

    public void updateRotationAxes(float x, float y)
    {
        _rotX = x;
        _rotY = y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_rotX);
        Debug.Log(_rotY);
        _rotX += Input.GetAxis("Mouse X") * _rotXSpeed;
        _rotY += Input.GetAxis("Mouse Y") * _rotYSpeed;

        _rotY = Mathf.Clamp(_rotY ,-_rotYMaxAngle, _rotYMaxAngle);

        Quaternion xQuat = Quaternion.AngleAxis(_rotX, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(_rotY, Vector3.left);
        _target.rotation = xQuat * yQuat;
    }
}
