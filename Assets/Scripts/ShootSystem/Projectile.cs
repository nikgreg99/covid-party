using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Rigidbody _rb;
    private float _range = 1;
    private float _startTime;
    private bool _homing;

    private bool isHit = false;
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
        if (_homing)
        {
            Vector3 pos = transform.position;
            List<Collider> nearColliders;

            nearColliders = new List<Collider>(Physics.OverlapSphere(pos, 10));
            if (nearColliders.Count > 0)
            {
                nearColliders = nearColliders
                    .Where(c => c.transform.gameObject.GetComponent<AIMovement>() != null)
                    .OrderBy(c => (c.transform.position - pos).magnitude)
                    .ToList();
                if (nearColliders.Count > 0)
                {
                    Vector3 nearEnemyPos = nearColliders[0].transform.position;
                    Vector3 newDir = (nearEnemyPos - pos).normalized;
                    float curMagnitude = _rb.velocity.magnitude;
                    Vector3 newVelocity = (_rb.velocity + newDir * curMagnitude).normalized * curMagnitude;
                    _rb.velocity = newVelocity;
                }
            }
        }




        if (Time.time - _startTime > _range)
        {
            _rb.useGravity = true;
            _rb.AddForce(Vector3.down * 10, ForceMode.Acceleration);
        }
    }

    public void SetRange(float range)
    {
        _range = range;
    }
    public void SetHoming(bool homing)
    {
        _homing = homing;
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
        else if (collision.gameObject.GetComponent<AIMovement>() != null && !isHit)
        {
            isHit = true;
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<AIMovement>().TargetHit();
        }
    }
}
