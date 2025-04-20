using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform Target;
    void Update()
    {
        transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);
    }
}
