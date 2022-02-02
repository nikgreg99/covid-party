using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccinatedShoot : MonoBehaviour
{

    [SerializeField] GameObject _launcher;
    [SerializeField] GameObject _syringe;
    [SerializeField] LayerMask _playerMask;
    [SerializeField] float _speed = 10f;

    private float _timer = 1f;
    private float _counter = 0;
    private Transform _playerTrans;
    private AIMovement _ai;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _playerTrans = GameObject.FindWithTag("Player").transform;
        _ai = GetComponent<AIMovement>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        _counter += Time.deltaTime;

        if (_ai.PlayerOnSight && !_enemyHealth.Infected && _timer < _counter)
        {
            Vector3 fromEnemyToPlayer = (_playerTrans.position - _launcher.transform.position).normalized;
            GameObject projectile = GameObject.Instantiate(_syringe, _launcher.transform.position, Quaternion.FromToRotation(Vector3.down, fromEnemyToPlayer));
            Rigidbody projRb = projectile.GetComponent<Rigidbody>();
            projRb.useGravity = false;
            projRb.velocity = fromEnemyToPlayer * _speed;
            _counter = 0;
        }
    }

}
