using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float wanderRadius = 1000f;
    [SerializeField] private float wanderTime = 10f;
    [SerializeField] private float minDistanceToPlayer = 10f;
    [SerializeField] private float multiplyBy = 3f;

    private Transform target;
    private List<Animator> _animators = new List<Animator>();
    private float timer;
    private float speed;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }
        agent.enabled = false;
        agent.baseOffset = 2;
        agent.height = 0.5f;
        timer = wanderTime;
        speed = agent.speed;
    }

    private void Update()
    {

        if (agent.enabled)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTime)
            {
                foreach (Animator a in _animators)
                {
                    a.SetBool("moving", true);
                }

                if (Vector3.Distance(target.position, transform.position) < minDistanceToPlayer)
                {
                    Vector3 newPos = (transform.position - target.position).normalized * multiplyBy;
                    agent.speed = 2 * speed;
                    agent.SetDestination(newPos);
                }
                else
                {
                    Vector3 newPos = RandomMove(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    agent.speed = speed;
                }
                timer = 0;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            agent.enabled = true;
            body.isKinematic = true;
            body.useGravity = false;
            Debug.Log("Touched ground");
        }
    }

    public Vector3 RandomMove(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;
        randDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, dist, layermask);

        return navHit.position;
    }

}
