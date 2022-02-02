using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(Wait());
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
        int layer = collision.gameObject.layer;
        if(layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Obstacle"))
        {
            StartCoroutine(destroyRoutine(1));
        }else if (layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(10);
            Destroy(this.gameObject);
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        _rb.useGravity = true;

        yield return null;
    }
}
