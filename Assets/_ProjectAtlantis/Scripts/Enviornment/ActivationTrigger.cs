using UnityEngine;
using UnityEngine.Events;

public class ActivationTrigger : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private bool destroyAfterTriggering = true;
    [SerializeField] private UnityEvent activationEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerTag))
        {
            activationEvent?.Invoke();
            if (destroyAfterTriggering)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    transform.GetChild(i).parent = null;
                }
                Destroy(gameObject);
            }
        }
    }
}