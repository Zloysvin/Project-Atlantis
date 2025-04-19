using UnityEngine;

public class BodyRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;
    Vector2 direction;
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
