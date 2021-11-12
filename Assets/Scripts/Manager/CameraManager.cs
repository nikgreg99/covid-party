using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private bool _isFirstPerson = false;
    [SerializeField] private GameObject firstCameera;
    [SerializeField] private GameObject orbitCamera;


    private void Start()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        if (_isFirstPerson)
        {
             firstCameera.SetActive(true);
             orbitCamera.SetActive(false);
        } 
        else
        {
            firstCameera.SetActive(false);
             orbitCamera.SetActive(true);
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
