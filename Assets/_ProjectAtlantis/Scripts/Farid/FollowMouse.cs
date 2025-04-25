using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Transform followObject;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 input = Input.mousePosition;
        if (float.IsNaN(input.x) || float.IsInfinity(input.x) || float.IsNaN(input.y) || float.IsInfinity(input.y)) { input = Vector2.zero; }
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(input);
        targetPos.z = 0f;

        followObject.position = targetPos;
    }
}