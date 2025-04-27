using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class SonarPing : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private float deathSpeed;
    private SpriteRenderer mat;

    SpriteRenderer spriteRenderer;
    bool gotReleased = false;
    void Awake()
    {
        deathSpeed = 1f / lifeTime;

        mat = GetComponent<SpriteRenderer>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
    }

    void Update()
    {
        if (mat.color.a <= 0f)
        {
            Destroy(gameObject);
        }

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - deathSpeed * Time.deltaTime);
    }
}
