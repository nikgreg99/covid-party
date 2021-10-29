using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private float _range = 1;
    private float _startTime;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _startTime > _range)
        {
            _rb.useGravity = true;
        }
    }

    public void SetRange(float range)
    {
        _range = range;
    }

    private IEnumerator destroyRoutine(int waitSecs)
    {
        yield return new WaitForSeconds(0.2f);
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(waitSecs);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collisionLayer = collision.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Ground") || collisionLayer == LayerMask.NameToLayer("Obstacle"))
        {
            StartCoroutine(destroyRoutine(1));
        }
    }
}
