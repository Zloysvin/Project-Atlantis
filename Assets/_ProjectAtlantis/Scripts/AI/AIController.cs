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
        soundEmmiter = GetComponent<MonsterSoundEmmiter>();
        StartCoroutine("AISoundTest");
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

        transform.right = new Vector3(agent.velocity.x, agent.velocity.y) + transform.position;
    }

    public void CheckAgro(Vector3 target, float distance)
    {
        if (distance <= agroRange)
        {
            // AI become aggressive towards player sub
        }
    }

    private IEnumerator AISoundTest()
    {
        while (true)
        {
            soundEmmiter.SendSound();
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }
}
