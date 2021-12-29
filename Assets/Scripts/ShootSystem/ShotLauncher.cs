using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShotLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private int _shotgunBulletCount = 5;
    [SerializeField] private float _posXOffset = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void GenerateShot(float shotSpeed, float range, float shotSize, bool dualShoot, bool isShotgun, bool homing)
    {
        int individualShotCount = dualShoot ? 2 : 1;
        int shotGunCount = isShotgun ? _shotgunBulletCount : 1;
        if (_projectilePrefab != null)
        {
            for (int i = 0; i < shotGunCount; i++)
            {
                for (int j = 0; j < individualShotCount; j++)
                {
                    float offset = isShotgun ? 0 : j * _posXOffset;
                    IndividualShot(shotSpeed, range, shotSize, offset, isShotgun, homing);
                }
            }
        }
        else
        {
            Debug.LogError("Missing Projectile Prefab");
        }
    }


    private void IndividualShot(float shotSpeed, float range, float shotSize, float posXOffset, bool isShotGun, bool homing)
    {
        Vector3 shotOrientation = CameraManager.currentCamera.transform.forward * shotSpeed;
        Vector3 rightVector = Vector3.Cross(shotOrientation, Vector3.up);

        if (isShotGun)
        {
            shotOrientation = Quaternion.AngleAxis(Random.Range(-10f, 10f), Vector3.up) * shotOrientation;
            shotOrientation = Quaternion.AngleAxis(Random.Range(-10f, 10f), rightVector) * shotOrientation;
        }

        GameObject proj = GameObject.Instantiate(_projectilePrefab);
        proj.transform.position = transform.position + rightVector.normalized * posXOffset;
        proj.transform.localScale = proj.transform.localScale * shotSize;
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        Projectile projectileComponent = proj.GetComponent<Projectile>();
        projectileComponent.SetRange(isShotGun ? range / 2 : range);
        projectileComponent.SetHoming(homing);

        rb.velocity = shotOrientation;
    }
}
