using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTime = 5f;

    private Rigidbody body;
    private NavMeshAgent agent;
    private List<Animator> _animators = new List<Animator>();
    private float timer;

    private void Start()
    {
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }
        body = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.baseOffset = 2;
        agent.height = 0.5f;
        timer = wanderTime;
    }

    private void Update()
    {

        if (agent.enabled)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTime)
            {
                Vector3 newPos = RandomMove(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);

                foreach (Animator a in _animators)
                {
                    a.SetBool("moving", true);
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
