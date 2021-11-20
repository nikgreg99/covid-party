using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    [SerializeField] private float _rotSpeed = 1.5f;
    [SerializeField] private float _maxAngleRotationX = 90.0f;
    [SerializeField] private Transform _player;

    private float _rotX = 0.0f;
    private float _rotY = 0.0f;

    private void OnEnable()
    {
        Quaternion orbitRotation = CameraManager.orbitRotation;
        if (orbitRotation != null)
        {
            transform.rotation = orbitRotation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPaused)
        {
            float mouseX = Input.GetAxis("Mouse X") * _rotSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * _rotSpeed;

            //inversione da asse mouse a asse attorno a cui girare
            _rotY += mouseX;
            _rotX += mouseY;

            _rotX = Mathf.Clamp(_rotX, -_maxAngleRotationX, _maxAngleRotationX);

            transform.rotation = Quaternion.Euler(_rotX, _rotY, 0.0f);
            //_player.Rotate(Vector3.up * mouseX);
        }

    }
}
