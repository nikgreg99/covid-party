using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _speedRotate = 50.0f;
    [SerializeField] private float _speedMultiplier = 5.0f;
    [SerializeField] private float _mouseSensitiity = 5.0f;

    private Rigidbody _rigidbody;

    private Vector3 _translation = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;

    private bool _mouseMode = false;
    private bool _speedUp = false;

    private List<Animator> _animators = new List<Animator>();
    private bool _lastMoving = false;

    private void UpdateTranslation(float moveX, float moveZ)
    {
        _translation = Vector3.zero;
        _translation += transform.right * moveX;
        _translation += transform.forward * moveZ;

        if (_speedUp)
        {
            _translation *= _speedMultiplier;
        }
    }

    private void UpdateRotation()
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
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotationY = -1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                rotationY = 1;
            }
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
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool moving = moveX != 0 || moveZ != 0;

        if (_lastMoving != moving)
        {
            _lastMoving = moving;

            foreach (Animator a in _animators)
            {
                a.SetBool("moving", moving);
            }
        }




        _speedUp = Input.GetKeyDown(KeyCode.LeftShift);

        UpdateTranslation(moveX, moveZ);
        UpdateRotation();
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _translation * _speed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_rotation * _speedRotate * Time.fixedDeltaTime));
    }
}




