using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _speedRotate = 50.0f;
    [SerializeField] private float _speedMultiplier = 3.0f;
    [SerializeField] private float _mouseSensitiity = 5.0f;
    [SerializeField] private float _camPlayerSlerpFactor = 1f;

    private Rigidbody _rigidbody;

    private Vector3 _translation = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;

    private bool _mouseMode = false;

    private List<Animator> _animators = new List<Animator>();
    private bool _lastMoving = false;

    [SerializeField] private OrbitCamera _orbitCamera;

    private Vector3 targetRotation;
    private Vector3 startRotation;
    private float _startTime;

    private void UpdateTranslation(float moveX, float moveZ)
    {
        _translation = Vector3.zero;
        _translation += transform.right * moveX;
        _translation += transform.forward * moveZ;
    }

    private void UpdateRotation()
    {
        if (_speedUp)
        {
            _rotation *= _speedMultiplier;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //_orbitCamera = GetComponentInChildren<OrbitCamera>();
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

        float camYRotation = _orbitCamera.gameObject.transform.rotation.eulerAngles.y;
        float playerYRotation = transform.rotation.eulerAngles.y;
        if (Mathf.Abs(camYRotation - playerYRotation)>=5 && (targetRotation == null || Mathf.Abs(targetRotation.y - camYRotation)>=5))
        {
            targetRotation = new Vector3(0, camYRotation, 0);
            startRotation = new Vector3(0,playerYRotation, 0);
            _startTime = Time.time;
        }

        float progress = (Time.time - _startTime) * _camPlayerSlerpFactor;
        if (progress <= 1)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetRotation) , progress);
        }


        _speedUp = Input.GetKey(KeyCode.LeftShift);

        UpdateTranslation(moveX, moveZ);
        UpdateRotation();
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _translation * _speed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_rotation * _speedRotate * Time.fixedDeltaTime));
    }
}





