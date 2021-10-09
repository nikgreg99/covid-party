using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _speedRotate = 5.0f;
    [SerializeField] private float _speedMultiplier = 5.0f;
    [SerializeField] private float _mouseSensitiity = 5.0f;

    private Rigidbody _rigidbody;

    private Vector3 _translation = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;

    private bool _mouseMode = false;
    private bool _speedUp = false;

    private void updateTranslation(float moveX, float moveZ)
    {
        _translation = Vector3.zero;
        _translation += Vector3.right * moveX;
        _translation += Vector3.forward * moveZ;

        if (_speedUp)
        {
            _translation *= _speedMultiplier;
        }
    }

    private void updateRotation()
    {
        float rotationY = 0;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _mouseMode = !_mouseMode;
        }

        if (_mouseMode)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            rotationY += Input.GetAxis("Mouse X") * _mouseSensitiity;
        }
        _rotation = Vector3.up * rotationY;

        if (_speedUp)
        {
            _rotation *= _speedMultiplier;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        _speedUp = Input.GetKeyDown(KeyCode.LeftShift);

        updateTranslation(moveX, moveZ);
        updateRotation();
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position +_translation * _speed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_rotation * _speedRotate * Time.fixedDeltaTime));
    }
}
 
   

   
