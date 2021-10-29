using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private bool _isFirstPerson = false;
    [SerializeField] private GameObject _firstCamera;
    [SerializeField] private GameObject _orbitCamera;


    private void Start() {

        UpdateView();
    }

    private void UpdateView()
    {
  
        if (_isFirstPerson)
        {
            _firstCamera.SetActive(true);
            _orbitCamera.SetActive(false);
        }
        else
        {
            _firstCamera.SetActive(false);
            _orbitCamera.SetActive(true);
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
