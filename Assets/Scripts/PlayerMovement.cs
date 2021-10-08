using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    private Rigidbody _rigidbody;

    [SerializeField]private float upLimit = 3.0f;
    [SerializeField]private float downLimit = 0.0f;

    private Vector3 _position;
    private float _direction = 1.0f;

    private void moveMainPlayer()
    {

        if (transform.position.y < downLimit)
        {
            _direction = 1.0f;
        }
        else if (transform.position.y > upLimit)
        {
            _direction = -1.0f;
        }

        _position += Time.fixedDeltaTime * _speed *  _direction * Vector3.forward;
        _rigidbody.MovePosition(_position);
    }

    void Awake()
    {
        _position = transform.localPosition;
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        moveMainPlayer();
    }
}
 
   

   
