using System.Collections;
using UnityEngine;

public class GameOverEffect : MonoBehaviour
{
    public static GameOverEffect Instance;
    [SerializeField] private float reloadDuration = 10f;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameFinishScreen;
    [SerializeField] private Material screenOverlay;

    [Header("Material manipulation related:")]
    [SerializeField] private Vector2 pixelationChangeInterval = new Vector2(0.1f, .5f);

    [SerializeField] private Vector2Int pixelationAmountRange = new Vector2Int(4, 120);

    private bool isBusy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        screenOverlay.DisableKeyword("PIXELATE_ON");
    }

    [ContextMenu("TestDeath")]
    public void TriggerDeathEffect()
    {
        if (isBusy) return;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        isBusy = true;
        float timer = reloadDuration;
        screenOverlay.EnableKeyword("PIXELATE_ON");
        gameOverScreen.SetActive(true);

        while (timer > 0)
        {
            screenOverlay.SetInt("_PixelateSize", Random.Range(pixelationAmountRange.x, pixelationAmountRange.y));
            float interval = Random.Range(pixelationChangeInterval.x, pixelationChangeInterval.y);
            timer -= interval;
            yield return new WaitForSeconds(interval);
        }
        SceneLoader.Instance.LoadScene(0);

        screenOverlay.DisableKeyword("PIXELATE_ON");
    }

    [ContextMenu("TestDeath")]
    public void TriggerFinishEffect()
    {
        if (isBusy) return;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayFinishAnimation()
    {
        isBusy = true;
        float timer = reloadDuration;
        screenOverlay.EnableKeyword("PIXELATE_ON");
        gameFinishScreen.SetActive(true);

        while (timer > 0)
        {
            screenOverlay.SetInt("_PixelateSize", Random.Range(pixelationAmountRange.x, pixelationAmountRange.y));
            float interval = Random.Range(pixelationChangeInterval.x, pixelationChangeInterval.y);
            timer -= interval;
            yield return new WaitForSeconds(interval);
        }
        SceneLoader.Instance.LoadScene(0);

        screenOverlay.DisableKeyword("PIXELATE_ON");
    }

    private void OnDisable()
    {
        StopAllCoroutines();

    }
}