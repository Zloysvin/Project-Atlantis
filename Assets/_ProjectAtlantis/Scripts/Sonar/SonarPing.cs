using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class SonarPing : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private float deathSpeed;
    private SpriteRenderer mat;

    ObjectPool<SonarPing> pool;

    //SpriteRenderer spriteRenderer;
    //Tweener alphaTween;
    bool gotReleased = false;
    void Awake()
    {
        deathSpeed = 1f / lifeTime;

        mat = GetComponent<SpriteRenderer>();

        //spriteRenderer = GetComponent<SpriteRenderer>();

        //alphaTween = spriteRenderer.DOFade(0f, lifeTime).From(1f).Pause();
    }
    private void OnEnable()
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
        //alphaTween.Rewind();
        //alphaTween.Play();
        gotReleased = false;
    }

    void FixedUpdate()
    {
        //if (mat.color.a == 0f)
        //{
        //    Destroy(gameObject);
        //}

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - deathSpeed * Time.fixedDeltaTime);
        if (mat.color.a <= 0f)
        {
            //mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0f);
            if (!gotReleased)
            {
                pool.Release(this);
                gotReleased = true;
            }
        }
    }

    public void SetPool(ObjectPool<SonarPing> pool)
    {
        this.pool = pool;
    }

    //private void OnDestroy()
    //{
    //    transform.DOKill(spriteRenderer);
    //}
}
