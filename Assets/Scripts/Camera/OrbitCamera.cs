using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _rotSpeed = 1.5f;
    [SerializeField] private float _maxAngleRotationX = 360.0f;

    [SerializeField] private float _offsetZ = -7;
    [SerializeField] private float _offsetY = 1;

    [SerializeField] private float _targetXOffset = 1;
    [SerializeField] private float _targetYOffset = 1;

    private CameraManager _cameraManager;

    private float _rotY = 0;
    private float _rotX = 0;
    private Vector3 _offset;


    // Start is called before the first frame update
    void Start()
    {
        _cameraManager = GetComponentInParent<CameraManager>();
        transform.position = _target.position + _offsetZ * _target.forward + _offsetY * _target.up;
        _offset = _target.position - transform.position;
        SetLookAt();

        _rotX = transform.rotation.x;
        _rotY = transform.rotation.y;

        _cameraManager.CurRotX = _rotX;
        _cameraManager.CurRotX = _rotX;

#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    private void OnEnable()
    {
        if (_cameraManager != null)
        {
            _rotY = _cameraManager.CurRotY;
            _rotX = _cameraManager.CurRotX;
        }

    }


    void LateUpdate()
    {
        if (!PauseMenu.gameIsPaused)
        {
            _rotY += Input.GetAxis("Mouse X") * _rotSpeed;
            _rotX -= Input.GetAxis("Mouse Y") * _rotSpeed;

            _rotX = Mathf.Clamp(_rotX, -_maxAngleRotationX, _maxAngleRotationX);

            _cameraManager.CurRotX = _rotX;
            _cameraManager.CurRotY = _rotY;

            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
            transform.position = _target.position - (rotation * _offset);
            //GetComponentInParent<CameraManager>().setRotation(transform.rotation);
            SetLookAt();
        }

    }

    private void SetLookAt()
    {
        Vector3 lookAt = _target.position;
        lookAt += _targetXOffset * _target.right + _targetYOffset * _target.up;
        transform.LookAt(lookAt, _target.up);
    }
}
