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

    [SerializeField] private float minShotDelay = 0.25f;
    [SerializeField] private float _shotDelay = 0.5f;
    [SerializeField] private float _shotSpeed = 2f;
    [SerializeField] private float _range = 4f;
    [SerializeField] private float _shotSize = 2f;
    [SerializeField] private int _damage = 20;
    [SerializeField] private AudioSource _playerSource;
    [SerializeField] private AudioClip _coughAudio;
    [SerializeField] private AudioClip _megaCoughAudio;

    private int damageCounter = 0;

    private bool _dualShoot = false;
    private bool _isShotgun = false;
    private bool _homing = false;

    private void powerUpAcquired(PowerUp powerUp)
    {
        switch (powerUp.PowerupType)
        {
            case PowerupTypes.DOUBLE_SHOT:
                _dualShoot = true;
                break;
            case PowerupTypes.HOMING:
                _homing = true;
                break;
            case PowerupTypes.RANGE_UP:
                _range *= 1.15f;
                break;
            case PowerupTypes.SHOOT_RATE_UP:
                _shotDelay = minShotDelay + (_shotDelay - minShotDelay) / 1.3f;
                break;
            case PowerupTypes.SHOTGUN:
                if (!_isShotgun)
                {
                    _isShotgun = true;
                    _shotDelay *= 2;
                }
                break;
            case PowerupTypes.SHOT_SPEED_UP:
                _shotSpeed *= 1.3f;
                break;
            case PowerupTypes.DAMAGE_UP:
                damageCounter++;
                float coefficient = damageCounter switch
                {
                    var n when n > 16 => 1.1f,
                    var n when n > 8 && n <= 16 => 1.2f,
                    _ => 1.3f
                };

                _damage = Mathf.CeilToInt(_damage * coefficient);
                //shot size
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
            _launcher.GenerateShot(_shotSpeed, _range, _shotSize, _dualShoot, _isShotgun, _homing, _damage);
            _playerSource.pitch = UnityEngine.Random.Range(0.85f, 1.15f);

            if (_isShotgun)
            {
                _playerSource.PlayOneShot(_megaCoughAudio);
            }
            else
            {
                _playerSource.PlayOneShot(_coughAudio);
            }

        }
    }
}
