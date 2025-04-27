using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform Target;

    private Vector3 targetStartingPos;
    private Vector3 cameraStartPos;

    void Awake()
    {
        targetStartingPos = Target.position;
        cameraStartPos = transform.position;
    }

    void Update()
    {
        Vector3 updPos = Target.position - targetStartingPos;
        transform.position = new Vector3(cameraStartPos.x + updPos.x, cameraStartPos.y + updPos.y, transform.position.z);
    }
}
