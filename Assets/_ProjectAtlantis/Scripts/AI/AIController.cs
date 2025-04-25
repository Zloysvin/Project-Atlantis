using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AIController : MonoBehaviour
{
    public enum AIState
    {
        Patrol, Investigation, Agro
    }

    public List<Transform> PatrolPoints;

    [Header("Move Stats")]
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float moveSpeed = 2f;

    [Header("AI Behavior")]
    [SerializeField] private float agroRange = 10f;
    [SerializeField] private float agroTime = 10f;
    [SerializeField] private AIState state;
    [SerializeField] private bool SoundReload = false;

    [Header("AI Orbit Parameters")]
    [SerializeField] private int maxOrbitPoints;
    [SerializeField] private int baseOrbitRadius;
    [SerializeField] private float minOrbitRadius;
    [SerializeField] private float navMeshEdgeOffset;
    private List<Vector3> orbitPositions;

    private NavMeshAgent agent;
    private int currentIndex = 0;

    private MonsterSoundEmmiter soundEmmiter;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.SetDestination(PatrolPoints[currentIndex].position);
        orbitPositions = new List<Vector3>();
        soundEmmiter = GetComponent<MonsterSoundEmmiter>();
        StartCoroutine("AISoundTest");
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (Mathf.Abs(Vector2.Distance(transform.position, PatrolPoints[currentIndex].position)) <= 2f)
        {
            currentIndex++;
            if (currentIndex == PatrolPoints.Count)
            {
                currentIndex = 0;
            }

            agent.SetDestination(PatrolPoints[currentIndex].position);
        }

        transform.right = new Vector3(agent.velocity.x, agent.velocity.y) + transform.position;
    }

    public void CheckAgro(Vector3 target, float distance)
    {
        if (distance <= agroRange)
        {
            // AI become aggressive towards player sub
        }

        GenerateDynamicOrbit(target);
    }

    private IEnumerator AISoundTest()
    {
        while (true)
        {
            soundEmmiter.SendSound();
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    private void MoveToPoint(Vector3 target)
    {

    }

    private void GenerateDynamicOrbit(Vector3 center)
    {
        orbitPositions.Clear();

        List<Vector3> bestPositions = new List<Vector3>();

        for (int i = 0; i < maxOrbitPoints; i++)
        {
            for (int j = baseOrbitRadius; j >= 0; j--)
            {
                Vector3 basePoint = Vector3.up * j;
                Vector3 rotatedPoint = Quaternion.AngleAxis(360f / maxOrbitPoints * i, Vector3.forward) * basePoint + center;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(rotatedPoint, out hit, navMeshEdgeOffset, NavMesh.AllAreas))
                {
                    bestPositions.Add(hit.position);
                    break;
                }
            }
        }

        if(bestPositions.Count > 0 )
        {
            for (int i = 0; i < bestPositions.Count - 1; i++)
            {
                Debug.DrawLine(bestPositions[i], bestPositions[i + 1], Color.green, 6f);
            }

            Debug.DrawLine(bestPositions[^1], bestPositions[0], Color.green, 6f);
        }
    }
}
