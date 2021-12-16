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
    [SerializeField] private float multiplyBy = 10f;

    public delegate void ScoreAction(int value);
    public static ScoreAction hit;
    public static ScoreAction incrementPassive;

    private Transform target;
    private List<Animator> _animators = new List<Animator>();
    private float timer;
    private float speed;
    private float angularSpeed;

    private float _infectionPercent = 0f;

    public void TargetHit()
    {
        if (_infectionPercent < 1)
        {
            _infectionPercent += 0.25f;
            ChangeInfectedStatus(_infectionPercent);
            hit(5);
            Debug.Log(_infectionPercent);
            if (_infectionPercent >= 1)
            {
                Debug.Log("full enemy");
                incrementPassive(1);
            }
        }
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }

        transform.position = new Vector3(transform.position.x, 20, transform.position.z);

        agent.enabled = false;
        body.isKinematic = false;
        body.useGravity = true;

        agent.baseOffset = 2;
        agent.height = 0.5f;
        timer = wanderTime;
        speed = agent.speed;
        angularSpeed = agent.angularSpeed;
    }

    private void Update()
    {

        if (agent.enabled)
        {
            timer += Time.deltaTime;

            float distance = Vector3.Distance(target.position, transform.position);

            if (distance < minDistanceToPlayer)
            {
                Vector3 newPos = transform.position + ((transform.position - target.position).normalized * multiplyBy);
                agent.speed = 2 * speed;
                agent.angularSpeed = 100 * angularSpeed;
                agent.SetDestination(newPos);
            }

            if (timer >= wanderTime && (distance > minDistanceToPlayer))
            {
                foreach (Animator a in _animators)
                {
                    a.SetBool("moving", true);
                }

                Vector3 newPos = RandomMove(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                agent.speed = speed;
                agent.angularSpeed = angularSpeed;
                
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
    private void ChangeInfectedStatus(float status)
    {
        status = Mathf.Clamp(status, 0, 1);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = new Color(status, 0, 0);
        }
    }

}
