using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _speedRotate = 50.0f;
    [SerializeField] private float _speedMultiplier = 3f;
    [SerializeField] private float _camPlayerSlerpFactor = 1f;

    [SerializeField] private float _jumpSpeed = 5.0f;
    [SerializeField] private float _offsetSpeed = 0.5f;
    [SerializeField] private float _gravity = 9.81f;
 
    private CharacterController _characterController;
    private CapsuleCollider _capsuleCollider;

    private Vector3 _translation = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;

    private List<Animator> _animators = new List<Animator>();
    private bool _lastMoving = false;

    private Vector3 targetRotation;
    private Vector3 startRotation;
    private float _startTime;
    private bool _speedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }

    }

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
        if (_speedUp)
        {
            _rotation *= _speedMultiplier;
        }
    }

    private bool checkJump()
    {
        bool _hitGround = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_capsuleCollider.height / 2) + 0.6f;
            _hitGround = hit.distance <= check;
        }
        return _hitGround;
    }

    private void jump(Vector3 movement)
    {
        movement.y = _jumpSpeed;
        movement.y -= _gravity * Time.deltaTime;

        _characterController.Move(movement * Time.deltaTime);
    }

    private void move()
    {
        Vector3 direction = _translation * _speed * Time.deltaTime;

         _characterController.Move(direction);
        _characterController.Move(Quaternion.Euler(_rotation * _speedRotate * Time.deltaTime).eulerAngles);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (checkJump())
            {
                jump(direction);
            }
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

        float camYRotation = CameraManager.currentCamera.gameObject.transform.rotation.eulerAngles.y;
        float playerYRotation = transform.rotation.eulerAngles.y;
        if (Mathf.Abs(camYRotation - playerYRotation) >= 5 && (targetRotation == null || Mathf.Abs(targetRotation.y - camYRotation) >= 5))
        {
            targetRotation = new Vector3(0, camYRotation, 0);
            startRotation = new Vector3(0, playerYRotation, 0);
            _startTime = Time.time;
        }

        float progress = (Time.time - _startTime) * _camPlayerSlerpFactor;
        if (progress <= 1)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetRotation), progress);
        }

        _speedUp = Input.GetKey(KeyCode.LeftShift);

        UpdateTranslation(moveX, moveZ);
        UpdateRotation();

        move();
    }

  
}





