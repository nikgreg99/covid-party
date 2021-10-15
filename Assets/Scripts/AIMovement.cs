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

        if (Physics.Raycast(transform.position, transform.forward, maxDistFromWall) || (counter > duration))
        {
            moveDir = chooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
            counter = 0.0f;
        }
    }

    Vector3 chooseDirection()
    {
        System.Random rand = new System.Random();
        int dir = rand.Next(0, 3);
        Vector3 temp = new Vector3();
        if(dir == 0)
        {
            temp = transform.forward;
        }
        else if (dir == 1)
        {
            temp = -transform.forward;
        }
        else if (dir == 2)
        {
            temp = transform.right;
        }
        else if (dir == 3)
        {
            temp = -transform.right;
        }
        return temp;
    }
}
