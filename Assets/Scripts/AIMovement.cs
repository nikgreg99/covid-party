using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public int speed = 1;
    private Rigidbody body;
    private Vector3 moveDir;
    public float maxDistFromWall = 0.0f;
    public float maxDistFromPlayer = 0.0f;
    private float counter = 0.0f;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        moveDir = chooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = moveDir * speed;
        counter += Time.deltaTime;
        RaycastHit hit;

        if ((body.SweepTest(transform.forward, out hit, maxDistFromWall) || counter > Random.Range(3.0f, 6.0f)) && (Vector3.Distance(transform.position, player.transform.position) > maxDistFromPlayer))
        {
            moveDir = Quaternion.Euler(0, Random.Range(100.0f, 190.0f), 0) * moveDir;
            transform.rotation = Quaternion.LookRotation(moveDir);
            counter = 0.0f;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            speed = 0;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < maxDistFromPlayer)
        {
            Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);
            moveDir = (playerPos - enemyPos).normalized;
            transform.rotation = Quaternion.LookRotation(moveDir);
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
}
