using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public float speed = 1.0f;
    private Rigidbody body;
    private Vector3 moveDir;
    public float maxDistFromWall = 0.0f;
    public float duration = 5.0f;
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
        RaycastHit hit;

        if (body.SweepTest(transform.forward, out hit, maxDistFromWall) || (counter > duration))
        {
            moveDir = chooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
            counter = 0.0f;
        }
    }

    Vector3 chooseDirection()
    {
        System.Random rand = new System.Random();
        int x = rand.Next(1, 10);
        int z = rand.Next(1, 10);
        int sign = rand.Next(0, 3);
        Vector3 dir = new Vector3();
        if(sign == 0)
        {
            dir = new Vector3(x, 0, z);
        } else if(sign == 1)
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
}
