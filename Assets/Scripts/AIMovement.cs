using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public float speed = 1.0f;
    private Rigidbody body;
    private Vector3 moveDir;
    public float maxDistFromWall = 0.0f;
    private float counter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        moveDir = chooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = moveDir * speed;
        counter += Time.deltaTime;
        float duration = Random.Range(1.0f, 5.0f);
        RaycastHit hit;

        if (body.SweepTest(transform.forward, out hit, maxDistFromWall) || (counter > duration))
        {
            moveDir = Quaternion.Euler(0, Random.Range(30.0f, 60.0f), 0) * moveDir;
            transform.rotation = Quaternion.LookRotation(moveDir);
            counter = 0.0f;
        }
    }

    Vector3 chooseDirection()
    {
        System.Random rand = new System.Random();
        float x = Random.Range(1.0f, 11.0f);
        float z = Random.Range(1.0f, 11.0f);
        int sign = rand.Next(1, 4);
        Vector3 dir = new Vector3();
        if (sign == 0)
        {
            dir = new Vector3(x, 0, z);
        }
        else if (sign == 1)
        {
            dir = new Vector3(-x, 0, z);
        }
        else if (sign == 2)
        {
            dir = new Vector3(-x, 0, -z);
        }
        else if (sign == 3)
        {
            dir = new Vector3(x, 0, -z);
        }
        return dir.normalized;
    }

    void lookingAround()
    {
        Debug.Log("Looking around");
    }
}
