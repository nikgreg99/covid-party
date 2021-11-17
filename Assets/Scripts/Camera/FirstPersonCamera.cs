using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    [SerializeField] private float _sensitivity = 30.0f;
    [SerializeField] private float _maxAngleRotationX = 45.0f;
    [SerializeField] private Transform _player;

    private float _rotX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        _rotX -= mouseY;
        _rotX = Mathf.Clamp(_rotX, -_maxAngleRotationX, _maxAngleRotationX);

        transform.localRotation = Quaternion.Euler(_rotX, 0.0f, 0.0f);
        _player.Rotate(Vector3.up * mouseX);

    }
}
