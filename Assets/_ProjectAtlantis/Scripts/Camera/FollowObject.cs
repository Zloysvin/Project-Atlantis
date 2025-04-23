using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform Target;

    private Vector3 targetStartingPos;

    void Awake()
    {
        targetStartingPos = Target.position;
        transform.position = new Vector3(0, 0, transform.position.z);
    }

    void Update()
    {
        Vector3 updPos = Target.position - targetStartingPos;
        transform.position = new Vector3(updPos.x, updPos.y, transform.position.z);
    }
}
