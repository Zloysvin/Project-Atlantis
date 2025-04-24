using System.Collections;
using UnityEngine;

public class ActiveSonar : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] private float angleMove = 1.5f;
    [SerializeField] private float sonarTime = 5f;
    [SerializeField] private GameObject pingProjectile;

    private float currentEmitterAngle = 0f;

    void Start()
    {
        StartCoroutine("Sonar");
    }

    private IEnumerator Sonar()
    {
        while (true)
        {
            // Sonar logic goes here

            Debug.Log("Sonar Active");
            PingDisplayHandler.Instance.DisplaySonarRing(sonarTime, 4f,transform.position);//speed should be the projectilespeed.

            while (currentEmitterAngle < 360f)
            {
                var projectile = Instantiate(pingProjectile, transform.position,
                    Quaternion.identity);

                projectile.transform.up = Quaternion.AngleAxis(currentEmitterAngle, Vector3.forward) * projectile.transform.up;

                currentEmitterAngle += angleMove;
            }

            currentEmitterAngle -= 360f;

            yield return new WaitForSeconds(sonarTime);
        }
    }
}
