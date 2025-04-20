using System.Collections;
using UnityEngine;

public class PingProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float deathTime = 5f;

    private void Start()
    {
        StartCoroutine("DeathTimer");
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PingDisplayHandler.Instance.DisplayPing(collision.contacts[0].point);
        Destroy(gameObject);
    }
}
