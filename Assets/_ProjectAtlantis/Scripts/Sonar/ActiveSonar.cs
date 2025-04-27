using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ActiveSonar : MonoBehaviour
{
    [Range(0f, 10f)] [SerializeField] private float angleMove = 1.5f;
    [SerializeField] private float sonarTime = 5f;
    [SerializeField] private PingProjectile pingProjectile;

    private SubmarineSoundEmmiter soundEmmiter;

    public bool IsPassiveSonar { get; private set; }

    private float currentEmitterAngle = 0f;


    void Start()
    {
        soundEmmiter = GetComponent<SubmarineSoundEmmiter>();

        StartCoroutine(Sonar());

        IsPassiveSonar = false;
    }

    private IEnumerator Sonar()
    {
        while (true)
        {
            if (!IsPassiveSonar)
            {
                PingDisplayHandler.Instance.DisplaySonarRing(sonarTime, 4, transform.position);
                // Sonar logic goes here

                //Debug.Log("Sonar Active");

                while (currentEmitterAngle < 360f)
                {
                    var projectile = Instantiate(pingProjectile, transform.position,
                        Quaternion.identity);

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
}
