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
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float viewRadius;
    [Range(0,360)]
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    public delegate void ScoreAction(int value);
    public static ScoreAction hit;
    public static ScoreAction incrementPassive;

    private Transform target;
    private List<Animator> _animators = new List<Animator>();
    private float timer;
    private float angularSpeed;

    private float _infectionPercent = 0f;

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
        agent.speed = speed;
        angularSpeed = agent.angularSpeed;
    }

    bool findVisibleTarget()
    {
        Collider[] playerInVewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        Transform player = playerInVewRadius[0].transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
        {
            float distToPlayer = Vector3.Distance(transform.position, target.position);

            if(!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public void TargetHit()
    {
        if (_infectionPercent < 1)
        {
            _infectionPercent += 0.25f;
            ChangeInfectedStatus(_infectionPercent);
            hit(5);
            Debug.Log(_infectionPercent);
            if(_infectionPercent > 0)
            {
                Run();
            }
            if (_infectionPercent >= 1)
            {
                Debug.Log("full enemy");
                incrementPassive(2);
            }
        }
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

            if (distance < minDistanceToPlayer && findVisibleTarget())
            {
                Run();
            }

            if (timer >= wanderTime && (distance > minDistanceToPlayer))
            {

                Vector3 newPos = RandomMove(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                agent.speed = speed;
                agent.angularSpeed = angularSpeed;
                
                timer = 0;
            }
        }

    }

    private void Run()
    {
        Vector3 newPos = transform.position + ((transform.position - target.position).normalized * multiplyBy);
        agent.speed = 3 * speed;
        agent.angularSpeed = 100 * angularSpeed;
        agent.SetDestination(newPos);
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
