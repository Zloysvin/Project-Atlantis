using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public float rotationSpeed = 15f;
    Vector2 direction;
    public float moveSpeed = 10f;

    public Transform target;
    // Update is called once per frame
    void Update()
    {
        if(target != null)
            direction = target.position-transform.position;
        else
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = target != null ? target.position : currentPos;
        transform.position = Vector2.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
    }
}
