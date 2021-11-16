using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    Rigidbody body;
    GameObject player;

    Vector3 moveDir;
    public int speed = 1;
    int initialSpeed = 0;

    public float maxDistFromObstacle = 0.0f;
    public float maxDistFromPlayer = 0.0f;
    float counter = 0.0f;

    public LayerMask groundMask;
    public LayerMask obstacleMask;

    private List<Animator> _animators = new List<Animator>();

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        moveDir = chooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
        player = GameObject.FindGameObjectWithTag("Player");
        initialSpeed = speed;

        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Animator a in _animators)
        {
            a.SetBool("moving", true);
        }

        body.MovePosition(body.position + transform.forward * Time.deltaTime * speed);

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        counter += Time.deltaTime;

        if(Physics.CheckSphere(transform.position, maxDistFromObstacle, obstacleMask) && (distanceToPlayer < maxDistFromPlayer))
        {
            speed = 0;
        }
        else if ((Physics.CheckSphere(transform.position, maxDistFromObstacle, obstacleMask) || counter > Random.Range(10f, 15f)) && (distanceToPlayer > maxDistFromPlayer))
        {
            speed = initialSpeed;
            moveDir = chooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
            counter = 0.0f;
        }
        else if (distanceToPlayer < 5f)
        {
            speed = 0;
        }
        else if (distanceToPlayer < maxDistFromPlayer)
        {
            speed = initialSpeed;
            Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);
            moveDir = (enemyPos - playerPos).normalized;
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    Vector3 chooseDirection()
    {
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0;
        return dir.normalized;
    }
}
