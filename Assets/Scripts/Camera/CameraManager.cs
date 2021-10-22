using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private OrbitCamera _orbitCamera;
    private bool _isFirstPerson = false;
    // Start is called before the first frame update
    void Start()
    {
        _orbitCamera = GetComponent<OrbitCamera>();

    }

    private void ChangeView()
    {
        _isFirstPerson = !_isFirstPerson;
        UpdateView();
    }

    private void UpdateView()
    {

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
