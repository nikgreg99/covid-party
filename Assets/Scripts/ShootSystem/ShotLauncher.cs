using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void GenerateShot(float shotSpeed, float range,float shotSize)
    {
        if (_projectilePrefab != null)
        {
            GameObject proj = GameObject.Instantiate(_projectilePrefab);
            proj.transform.position = transform.position;
            proj.transform.localScale = proj.transform.localScale * shotSize;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            proj.GetComponent<Projectile>().SetRange(range);
            rb.velocity = CameraManager.currentCamera.transform.forward * shotSpeed;
        }
        else
        {
            Debug.LogError("Missing Projectile Prefab");
        }
    }
}
