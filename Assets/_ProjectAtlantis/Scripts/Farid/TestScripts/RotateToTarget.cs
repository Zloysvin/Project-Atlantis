using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public float rotationSpeed = 15f;
    Vector2 direction;
    public float moveSpeed = 10f;

    public Transform target;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 input = Input.mousePosition;
        if (float.IsNaN(input.x) || float.IsInfinity(input.x) || float.IsNaN(input.y) || float.IsInfinity(input.y)) { input = Vector2.zero; }

        if (target != null)
            direction = target.position-transform.position;
        else
            direction = cam.ScreenToWorldPoint(input) -transform.position;
        float angle = Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        Vector2 currentPos = cam.ScreenToWorldPoint(input);
        Vector2 pos = target != null ? target.position : currentPos;
        transform.position = Vector2.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
    }
}
