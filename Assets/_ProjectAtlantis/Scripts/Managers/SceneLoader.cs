using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Image FadeImage;
    public float FadeInDuration = 1f;
    public float FadeOutDuration = 2.2f;

    bool isBusy;

    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isBusy = true;
        FadeImage.DOFade(0f, FadeInDuration).From(1f).OnComplete(() => isBusy = false);
    }

    public void LoadScene(int index)
    {
        if(isBusy) return;
        isBusy = true;

        FadeImage.DOFade(1f, FadeOutDuration).From(0f).OnComplete(() => { SceneManager.LoadScene(index); });        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        if (FadeImage != null) FadeImage.DOKill();
    }
}
