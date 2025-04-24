using UnityEngine;

public class FallingRockTraps : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TriggerTrap()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
