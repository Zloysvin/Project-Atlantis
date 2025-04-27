using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private int CharactersPerSecond;

    public static TypeWriter Instance;
    public bool PlaySound = false;
    private AudioSource typingSound;

    private string text;
    private TMP_Text textBox;

    private Coroutine typewriter;
    private WaitForSeconds delay;
    private int currentVisibleIndex = 0;
    private bool typeWriterIsActive = false;

    private Queue<(TMP_Text, string)> displayQueue = new Queue<(TMP_Text, string)>();

    private void Awake()
    {
        delay = new WaitForSeconds(1f / CharactersPerSecond);

        TypeWriter.Instance = this;

        if (PlaySound)
        {
            typingSound = GetComponent<AudioSource>();
            typingSound.loop = true;
        }
    }

    private void Start()
    {
    }

    public void StartTypeWriter(TMP_Text logLine, string text)
    {
        if(!typeWriterIsActive)
        {
            textBox = logLine;
            this.text = text;

            textBox.text = this.text;
            textBox.maxVisibleCharacters = 0;
            currentVisibleIndex = 0;

            typewriter = StartCoroutine(TypeWriteCoroutine());
        }
        else
        {
            displayQueue.Enqueue(new (logLine, text));
        }
    }

    public void StartTypeWriter(List<TMP_Text> textBoxes, List<string> texts)
    {
        var num = TypeWriteCoroutine(textBoxes, texts);

        typewriter = StartCoroutine(num);
    }

    public void StopTypeWriter()
    {
        StopAllCoroutines();
    }

    private IEnumerator TypeWriteCoroutine()
    {
        typeWriterIsActive = true;

        var textInfo = textBox.textInfo;

        while (currentVisibleIndex < textInfo.characterCount + 1)
        {
            char character = textInfo.characterInfo[currentVisibleIndex].character;

            textBox.maxVisibleCharacters++;
            currentVisibleIndex++;

            yield return delay;
        }

        typeWriterIsActive = false;

        if (displayQueue.Count > 0)
        {
            var waitingMember = displayQueue.Dequeue();
            StartTypeWriter(waitingMember.Item1, waitingMember.Item2);
        }
    }

    private IEnumerator TypeWriteCoroutine(List<TMP_Text> textBoxes, List<string> texts)
    {
        for (int i = 0; i < textBoxes.Count; i++)
        {
            textBox = textBoxes[i];
            text = texts[i];

            textBox.text = text;
            textBox.maxVisibleCharacters = 0;
            currentVisibleIndex = 0;

            var textInfo = textBox.textInfo;

            typingSound.Play();
            while (currentVisibleIndex < textInfo.characterCount + 1)
            {
                if (currentVisibleIndex < textInfo.characterInfo.Length)
                {
                    char character;
                    character = textInfo.characterInfo[currentVisibleIndex].character;
                }

                textBox.maxVisibleCharacters++;
                currentVisibleIndex++;

                yield return delay;
            }
            typingSound.Stop();

            yield return new WaitForSeconds(0.5f);
        }
    }
}
