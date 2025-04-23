using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AIController : MonoBehaviour
{
    public List<Transform> PatrolPoints;

    [Header("Move Stats")]
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float moveSpeed = 2f;

    private NavMeshAgent agent;
    private int currentIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.SetDestination(PatrolPoints[currentIndex].position);
    }

    private void Update()
    {
        if (Mathf.Abs(Vector2.Distance(transform.position, PatrolPoints[currentIndex].position)) <= 2f)
        {
            currentIndex++;
            if (currentIndex == PatrolPoints.Count)
            {
                currentIndex = 0;
            }

            agent.SetDestination(PatrolPoints[currentIndex].position);
        }
        Rotate(new Vector3(agent.velocity.x, agent.velocity.y));
    }

    private void Rotate(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
