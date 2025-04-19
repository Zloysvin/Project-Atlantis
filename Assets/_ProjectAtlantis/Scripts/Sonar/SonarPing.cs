using UnityEngine;

public class SonarPing : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private float deathSpeed;
    private SpriteRenderer mat;

    void Start()
    {
        deathSpeed = 1f / lifeTime;

        mat = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mat.color.a == 0f)
        {
            Destroy(gameObject);
        }

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - deathSpeed * Time.deltaTime);
        if (mat.color.a < 0f)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0f);
        }
    }
}
