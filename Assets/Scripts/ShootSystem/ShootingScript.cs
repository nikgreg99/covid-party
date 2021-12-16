using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerMovement.acquiredPowerup += powerUpAcquired;
    }


    private void OnDisable()
    {
        PlayerMovement.acquiredPowerup -= powerUpAcquired;
    }


    private float _lastShotTime = -10;
    [SerializeField] private float _shotDelay = 0.5f;
    [SerializeField] private float _shotSpeed = 2f;
    [SerializeField] private float _range = 4f;
    [SerializeField] private float _shotSize = 2f;
    private bool _dualShoot = false;
    private bool _isShotgun = false;
    private bool _homing = true;

    private void powerUpAcquired(PowerupTypes type)
    {
        switch (type)
        {
            case PowerupTypes.DOUBLE_SHOT:
                _dualShoot = true;
                break;
            case PowerupTypes.HOMING:
                _homing = true;
                break;
            case PowerupTypes.RANGE_UP:
                _range *= 1.3f;
                break;
            case PowerupTypes.SHOOT_RATE_UP:
                _shotDelay /= 1.3f;
                break;
            case PowerupTypes.SHOTGUN:
                _isShotgun = true;
                _shotDelay *= 2;
                break;
            case PowerupTypes.SHOT_SPEED_UP:
                _shotSpeed *= 1.3f;
                break;

        }
    }

    private ShotLauncher _launcher;
    // Start is called before the first frame update
    void Start()
    {
        _launcher = GetComponentInChildren<ShotLauncher>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0) && !PauseMenu.gameIsPaused)
        {
            Shoot();
        }


    }

    private void Shoot()
    {
        if (Time.time >= _lastShotTime + _shotDelay && _launcher != null)
        {
            _lastShotTime = Time.time;
            _launcher.GenerateShot(_shotSpeed, _range, _shotSize, _dualShoot, _isShotgun,_homing);
        }
    }
}
