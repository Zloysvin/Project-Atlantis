using UnityEngine;

public class MainMenuMonsterAI : MonoBehaviour
{
    [SerializeField] RotateToTarget[] monster;
    [SerializeField] Transform[] targetPoint;
    [SerializeField] float maxTravelDistance = 6f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < monster.Length; i++)
        {
            targetPoint[i].position = Random.insideUnitCircle * maxTravelDistance;
            monster[i].target = targetPoint[i];
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < monster.Length; i++)
        {
            if (Vector2.SqrMagnitude(monster[i].transform.position - targetPoint[i].position) <= 0.1f)
                targetPoint[i].position = Random.insideUnitCircle * maxTravelDistance;
        }
    }
}
