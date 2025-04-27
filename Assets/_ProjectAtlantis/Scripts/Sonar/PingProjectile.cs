using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PingProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float deathTime = 5f;

    ObjectPool<PingProjectile> pingPool;

    bool gotReleased = false;
    private void OnEnable()
    {
        StartCoroutine(DeathTimer());
        gotReleased = false;
    }

    private void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTime);
        //Destroy(gameObject);
        if (!gotReleased)
        {
            pingPool.Release(this);
            gotReleased = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PingDisplayHandler.Instance.DisplayPing(collision.contacts[0].point,transform.up);
        //Destroy(gameObject);
        if(!gotReleased)
        {
        pingPool.Release(this);
            gotReleased = true;
        }
    }

    public void SetPool(ObjectPool<PingProjectile> pool)
    {
        pingPool = pool;
    }
}
