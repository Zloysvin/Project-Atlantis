using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject HeaderText;

    public List<TMP_Text> BriefingParagraphs = new List<TMP_Text>();
    public List<string> BriefingTexts;

    public List<TMP_Text> KeyBindingParagraphs = new List<TMP_Text>();
    public List<string> KeyBindingTexts;

    private bool brienfingOpen = false;
    private bool keyBindingsOpen = false;

    public void OpenBriefing()
    {
        MainMenu.SetActive(false);
        HeaderText.SetActive(false);

        foreach (var paragrph in BriefingParagraphs)
        {
            paragrph.gameObject.SetActive(true);
        }

        TypeWriter.Instance.StartTypeWriter(BriefingParagraphs, BriefingTexts);
        StartCoroutine(KeyPressDefense());
    }

    public void OpenKeyBindings()
    {
        foreach (var paragrph in KeyBindingParagraphs)
        {
            paragrph.gameObject.SetActive(true);
        }

        TypeWriter.Instance.StartTypeWriter(KeyBindingParagraphs, KeyBindingTexts);
        StartCoroutine(KeyPressDefense2());
    }

    private IEnumerator KeyPressDefense()
    {
        brienfingOpen = false;
        yield return new WaitForSeconds(1.5f);
        brienfingOpen = true;
    }

    private IEnumerator KeyPressDefense2() // I don't have time for proper namings, I am sorry
    {
        keyBindingsOpen = false;
        yield return new WaitForSeconds(1.5f);
        keyBindingsOpen = true;
    }

    public void Update()
    {
        if (Input.anyKey)
        {
            if (keyBindingsOpen)
            {
                SceneLoader.Instance.LoadScene(1);
            }
            else if (brienfingOpen)
            {
                TypeWriter.Instance.StopTypeWriter();

                foreach (var paragraph in BriefingParagraphs)
                {
                    paragraph.gameObject.SetActive(false);
                }
                OpenKeyBindings();
            }
        }
    }
}
