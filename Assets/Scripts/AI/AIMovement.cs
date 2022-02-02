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

    [SerializeField] private Rigidbody _body;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _wanderRadius = 1000f;
    [SerializeField] private float _wanderTime = 10f;
    [SerializeField] private float _minDistanceToPlayer = 10f;
    [SerializeField] private float _multiplyBy = 10f;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _viewRadius;
    [Range(0, 360)]
    [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _playerMask;

    [SerializeField] private AITypes _aiType;
    [SerializeField] public AITypes AIType { get { return _aiType; } }

    public bool PlayerOnSight { get; private set; } = false;


    private Transform _target;
    private List<Animator> _animators = new List<Animator>();
    private float _timer;
    private float _angularSpeed;
    private bool _running = false;
    private EnemyHealth _enemyHealth;
    private bool _interactingWithPlayer = false;



    private void Start()
    {
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }

        _target = GameObject.FindWithTag("Player").transform;

        _agent.baseOffset = 2;
        _agent.height = 0.5f;

        _timer = _wanderTime;
        _agent.speed = _speed;
        _angularSpeed = _agent.angularSpeed;

        _enemyHealth = GetComponent<EnemyHealth>();
    }

    public bool findVisibleTarget()
    {
        Collider[] playerInVewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _playerMask);
        try
        {
            Transform player = playerInVewRadius[0].transform;

            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < _viewAngle / 2)
            {
                float distToPlayer = Vector3.Distance(transform.position, _target.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, _obstacleMask))
                {
                    return true;
                }
            }
        }
        catch (Exception)
        {
            //Debug.Log("Player outside view range");
        }

        return false;
    }

    private void Update()
    {

        if (_agent.enabled)
        {
            _timer += Time.deltaTime;

            EnableAnimation(true);

            float distance = Vector3.Distance(_target.position, transform.position);

            if (distance < _minDistanceToPlayer && (findVisibleTarget() || _running) && !_enemyHealth.Infected)
            {
                if (!_interactingWithPlayer)
                    _agent.ResetPath();

                _interactingWithPlayer = true;

                switch (_aiType)
                {
                    case AITypes.VACCINATED:
                        PlayerOnSight = true;
                        FollowPlayer(0.3f);
                        break;
                    case AITypes.ANTI_CONTAMINATION:
                        FollowPlayer(1.2f);
                        break;
                    default:
                        Run();
                        break;
                }
            }else if ((_timer >= _wanderTime || _interactingWithPlayer) && (distance > _minDistanceToPlayer || _enemyHealth.Infected || (!findVisibleTarget() && !_running)))
            {
                _running = false;
                PlayerOnSight = false;
                _interactingWithPlayer = false;

                //EnableAnimation(false);
                _agent.ResetPath();
                Vector3 newPos = RandomMove(transform.position, _wanderRadius, -1);
                _agent.speed = _speed;
                _agent.angularSpeed = _angularSpeed;
                _agent.SetDestination(newPos);
                //EnableAnimation(true);

                _timer = 0;
            }
        }

    }

    private void EnableAnimation(bool moving)
    {
        foreach (Animator a in _animators)
        {
            a.SetBool("moving", moving);
        }
    }

    public void NoticeHit()
    {
        float distance = Vector3.Distance(_target.position, transform.position);
        if (distance < _minDistanceToPlayer)
        {
            _agent.ResetPath();
            switch (_aiType)
            {
                case AITypes.VACCINATED:
                    FollowPlayer(0.3f);
                    break;
                case AITypes.ANTI_CONTAMINATION:
                    FollowPlayer(1.2f);
                    break;
                default:
                    Run();
                    break;
            }
        }
    }

    private void Run()
    {
        _running = true;

        Vector3 newPos = transform.position + (transform.position - _target.position).normalized * _multiplyBy;
        _agent.SetDestination(newPos);
        _agent.angularSpeed = 100 * _angularSpeed;
        _agent.speed = 3 * _speed;
    }

    private void FollowPlayer(float speedMultiplier)
    {
        _agent.SetDestination(_target.position);
        _agent.angularSpeed = 500 * _angularSpeed;
        _agent.speed = speedMultiplier * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _agent.enabled = true;
            _body.isKinematic = true;
            _body.useGravity = false;
        }
    }

    public Vector3 RandomMove(Vector3 origin, float dist, int layermask)
    {
        Vector2 randDir = UnityEngine.Random.insideUnitCircle.normalized * dist;
        Vector3 dir = new Vector3(randDir.x, 0, randDir.y);
        dir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(dir, out navHit, dist, layermask);

        return navHit.position;
    }

}
