using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class AIController : MonoBehaviour
{
    public enum AIState
    {
        Patrol, Investigation, Agro
    }

    public List<Transform> PatrolPoints;
    [Header("Monster Sounds")]
    [SerializeField] private List<AudioClip> SFXs = new List<AudioClip>();

    [SerializeField] private float soundRange = 10f;

    [Header("Move Stats")]
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float moveSpeed = 2f;

    [Header("AI Behavior")]
    [SerializeField] private float interestRange = 30f;
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

    private Vector3 target;
    private bool atTarget;
    private bool atOrbit;

    private int orbitIndex;
    private int totalOrbitIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        target = PatrolPoints[currentIndex].position;
        PatrolPoints[currentIndex].parent.parent = null;
        transform.right = (target - transform.position).normalized;
        agent.SetDestination(target);
        orbitPositions = new List<Vector3>();
        soundEmmiter = GetComponent<MonsterSoundEmmiter>();
        StartCoroutine("AIAmbientSound");
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);



        if (state == AIState.Patrol)
        {
            MoveToPoint(target);
            if(atTarget)
            {
                currentIndex++;
                if (currentIndex == PatrolPoints.Count)
                {
                    currentIndex = 0;
                }

                target = PatrolPoints[currentIndex].position;
                agent.SetDestination(target);
                //transform.right = new Vector3(agent.velocity.x, agent.velocity.y) + transform.position;
                transform.right = (target - transform.position).normalized;

                atTarget = false;
            }
        }

        if (state == AIState.Investigation)
        {
            if (!atOrbit)
            {
                MoveToPoint(target);
                if (atTarget)
                {
                    atOrbit = true;
                    orbitIndex = 0;
                    totalOrbitIndex = 0;
                    target = orbitPositions[0];
                    agent.SetDestination(target);
                    atTarget = false;
                }
            }
            else
            {
                MoveToPoint(target, 0.2f);
                if (atTarget)
                {
                    totalOrbitIndex++;
                    orbitIndex++;

                    if (orbitIndex == orbitPositions.Count)
                    {
                        orbitIndex = 0;
                    }

                    target = orbitPositions[orbitIndex];
                    agent.SetDestination(target);

                    if (totalOrbitIndex == orbitPositions.Count * 2)
                    {
                        state = AIState.Patrol;
                        atOrbit = false;
                        target = PatrolPoints[currentIndex].position;
                        agent.SetDestination(target);
                    }

                    atTarget = false;
                }
            }
        }
    }

    public void CheckAgro(Vector3 target, float distance)
    {
        if (distance <= interestRange)
        {
            state = AIState.Investigation;
            atOrbit = false;
            this.target = target;
            agent.SetDestination(target);

            GenerateDynamicOrbit(target);
        }
        else if(state == AIState.Investigation && distance <= agroRange)
        {
            state = AIState.Agro;
            atOrbit = false;
            this.target = target;
            agent.SetDestination(target);
        }
    }

    private IEnumerator AIAmbientSound()
    {
        while (true)
        {
            soundEmmiter.SendSound(SFXs[Random.Range(0, SFXs.Count)], soundRange);
            yield return new WaitForSeconds(Random.Range(10, 16));
        }
    }

    private IEnumerator AIDeAgro()
    {
        float elapsedTime = 0f;
        while (elapsedTime < agroTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        state = AIState.Patrol;
        atOrbit = false;
        target = PatrolPoints[currentIndex].position;
        agent.SetDestination(target);
    }

    private void MoveToPoint(Vector3 target)
    {
        if (Mathf.Abs(Vector2.Distance(transform.position, target)) <= 2f)
        {
            atTarget = true;
            return;
        }

        atTarget = false;

    }

    private void MoveToPoint(Vector3 target, float accuracy)
    {
        if (Mathf.Abs(Vector2.Distance(transform.position, target)) <= accuracy)
        {
            atTarget = true;
            return;
        }

        atTarget = false;
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

        orbitPositions = new List<Vector3>(bestPositions);
    }
}
