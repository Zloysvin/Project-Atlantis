using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ActiveSonar : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] private float angleMove = 1.5f;
    [SerializeField] private float sonarTime = 5f;
    [SerializeField] private PingProjectile pingProjectile;

    private SubmarineSoundEmmiter soundEmmiter;

    public bool IsPassiveSonar { get; private set; }

    private float currentEmitterAngle = 0f;


    public ObjectPool<PingProjectile> pingPool;

    void Start()
    {
        // Pool, true kann auf false gesetzt werden, wenn man sich sicher ist, dass sich das object nicht bereits im pool befindet.
        pingPool = new ObjectPool<PingProjectile>(CreatePing,OnTakePingProjectileFromPool,OnReturnPingProjectileToPool,OnDestroyPingProjectile,true,
            360,720);

        soundEmmiter = GetComponent<SubmarineSoundEmmiter>();

        StartCoroutine(Sonar());

        IsPassiveSonar = false;
    }

    private IEnumerator Sonar()
    {
        while (true)
        {
            if(!IsPassiveSonar)
            {
                PingDisplayHandler.Instance.DisplaySonarRing(sonarTime, 4, transform.position);
                // Sonar logic goes here

                //Debug.Log("Sonar Active");

                while (currentEmitterAngle < 360f)
                {
                    //var projectile = Instantiate(pingProjectile, transform.position,
                    //    Quaternion.identity);
                    var projectile = pingPool.Get();

                    projectile.transform.up = Quaternion.AngleAxis(currentEmitterAngle, Vector3.forward) *
                                              projectile.transform.up;

                    currentEmitterAngle += angleMove;
                }

                currentEmitterAngle -= 360f;

                SubmarineSoundsManager.Instance.EmitSonar();

                yield return new WaitForSeconds(sonarTime);
            }
            else
            {
                yield return null;
            }
        }
    }

    [ContextMenu("ToggleSonar")]
    public void TurnOnSonar(bool status)
    {
        IsPassiveSonar = status;
    }


    #region Pooling Methods

    private PingProjectile CreatePing()
    {
        PingProjectile ping = Instantiate(pingProjectile,transform.position, Quaternion.identity);
        ping.SetPool(pingPool);
        ping.transform.parent = PingDisplayHandler.Instance.transform;
        
        return ping;
    }

    private void OnTakePingProjectileFromPool(PingProjectile ping)
    {
        ping.transform.position = transform.position;
        ping.transform.rotation = Quaternion.identity;

        ping.gameObject.SetActive(true);
    }

    private void OnReturnPingProjectileToPool(PingProjectile ping)
    {
        ping.gameObject.SetActive(false);
    }

    // Whats happening when the object is destroyed instead of being send back to the pool.
    private void OnDestroyPingProjectile(PingProjectile ping)
    {
        Destroy(ping.gameObject);
    }

    #endregion
}
