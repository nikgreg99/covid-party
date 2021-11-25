using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Rigidbody body;
    private NavMeshAgent agent;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    private void Update()
    {
        if (agent.enabled)
        {
            agent.destination = target.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            agent.enabled = true;
            body.isKinematic = true;
            Debug.Log("Touched ground");
        }
    }
}
