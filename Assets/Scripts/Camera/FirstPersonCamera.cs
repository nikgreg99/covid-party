using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotYSpeed = 1.5f;
    [SerializeField] private float _rotYMaxAngle = 360.0f;

    private float rotY;

    // Start is called before the first frame update
    void Start()
    {
        rotY = _target.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        rotY += Input.GetAxis("Mouse Y") * _rotYSpeed;
        float clampedRotY = Mathf.Clamp(rotY, -_rotYMaxAngle, _rotYMaxAngle);

        Quaternion rotation = Quaternion.Euler(0, clampedRotY, 0);
        _target.rotation = rotation;
    }
}
