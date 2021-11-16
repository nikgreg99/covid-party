using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{

    private float _lastShotTime = -10;
    [SerializeField] private float _shotDelay = 0.5f;
    [SerializeField] private float _shotSpeed = 2f;
    [SerializeField] private float _range = 4f;
    [SerializeField] private float _shotSize = 2f;


    private ShotLauncher _launcher;
    // Start is called before the first frame update
    void Start()
    {
        _launcher = GetComponentInChildren<ShotLauncher>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0)&&!PauseMenu.gameIsPaused)
        {
            Shoot();
        }


    }

    private void Shoot()
    {
        if (Time.time >= _lastShotTime + _shotDelay && _launcher != null)
        {
            _lastShotTime = Time.time;
            _launcher.GenerateShot(_shotSpeed,_range,_shotSize);
        }
    }
}
