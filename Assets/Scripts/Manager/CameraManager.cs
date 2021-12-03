using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private bool _isFirstPerson = false;
    [SerializeField] private GameObject firstCameera;
    [SerializeField] private GameObject orbitCamera;
    public static Camera currentCamera { get; private set; }

    public float CurRotX { get; set; } = 0;
    public float CurRotY { get; set; } = 0;


    private void Start()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        if (_isFirstPerson)
        {
            //orbitRotation = orbitCamera.transform.rotation;
            firstCameera.SetActive(true);
            orbitCamera.SetActive(false);
            currentCamera = firstCameera.GetComponent<Camera>();
        }
        else
        {
            firstCameera.SetActive(false);
            orbitCamera.SetActive(true);
            currentCamera = orbitCamera.GetComponent<Camera>();
        }
    }

    


    private void ChangeView()
    {
        _isFirstPerson = !_isFirstPerson;
        UpdateView();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeView();
        }
    }
}
