using Unity.VisualScripting;
using UnityEngine;

public class TestArtifactTrigger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Collided with {other.transform.name}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
}
