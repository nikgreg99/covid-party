using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class AIMovement : MonoBehaviour
{
    public enum AITypes
    {
        NORMAL,
        MASKED,
        VACCINATED,
        ANTI_CONTAMINATION,
    }

    [SerializeField] private Rigidbody body;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float wanderRadius = 1000f;
    [SerializeField] private float wanderTime = 10f;
    [SerializeField] private float minDistanceToPlayer = 10f;
    [SerializeField] private float multiplyBy = 10f;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float viewRadius;
    [Range(0, 360)]
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private AITypes _aiType;
    [SerializeField] public AITypes AIType { get { return _aiType; } }


    private Transform target;
    private List<Animator> _animators = new List<Animator>();
    private float timer;
    private float angularSpeed;
    private bool isRunning = false;



    private void Start()
    {

        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }

        target = GameObject.FindWithTag("Player").transform;
        transform.position = new Vector3(transform.position.x, 20, transform.position.z);

        agent.baseOffset = 2;
        agent.height = 0.5f;

        timer = wanderTime;
        agent.speed = speed;
        angularSpeed = agent.angularSpeed;
    }

    bool findVisibleTarget()
    {
        Collider[] playerInVewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        try
        {
            Transform player = playerInVewRadius[0].transform;

            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float distToPlayer = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Player outside view range");
        }

        return false;
    }



    private void Update()
    {

        if (agent.enabled)
        {
            timer += Time.deltaTime;

            foreach (Animator a in _animators)
            {
                a.SetBool("moving", true);
            }

            float distance = Vector3.Distance(target.position, transform.position);

            if (distance < minDistanceToPlayer && (findVisibleTarget() || isRunning))
            {
                Run();
            }

            if (timer >= wanderTime && (distance > minDistanceToPlayer))
            {
                isRunning = false;

                Vector3 newPos = RandomMove(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                agent.speed = speed;
                agent.angularSpeed = angularSpeed;

                timer -= wanderTime;
            }
        }

    }

    public void RequestRun()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < minDistanceToPlayer)
        {
            Run();
        }
    }
    private void Run()
    {
        isRunning = true;

        Vector3 newPos = transform.position + (transform.position - target.position).normalized * multiplyBy;
        agent.SetDestination(newPos);
        agent.angularSpeed = 100 * angularSpeed;
        agent.speed = 3 * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            agent.enabled = true;
            body.isKinematic = true;
            body.useGravity = false;
            agent.SetDestination(RandomMove(transform.position, wanderRadius, -1));
        }
    }

    public Vector3 RandomMove(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * dist;
        randDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, dist, layermask);

        return navHit.position;
    }


}
