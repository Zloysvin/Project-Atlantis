using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StyleController : MonoBehaviour
{
    [SerializeField] bool keepChangesAfterGameMode;
    [Header("Materials to change colors:")]
    [SerializeField] Material buttonFG;
    [SerializeField] Material buttonBG;
    [SerializeField] Material buttonHighlight;
    [SerializeField] Material uiBase;
    [SerializeField] Material depthMeter;
    [SerializeField] Material player;
    [SerializeField] Material playerCollision;
    [SerializeField] Material enviornment;
    [SerializeField] Material ping;

    [Header("New Colors:")]
    [SerializeField] Color buttonFGColor;
    [SerializeField] Color buttonBGColor;
    [SerializeField] Color buttonHighlightColor;
    [SerializeField] Color uiBaseColor;
    [SerializeField] Color uiBaseColorBG;
    [SerializeField] Color uiHeaderColor;
    [SerializeField] Color depthMeterColor;
    [SerializeField] Color playerColor;
    [SerializeField] Color enviornmentColor;
    [SerializeField] Color pingColor;
    [SerializeField] Color textHeaderColor;
    [SerializeField] Color textInfoColor;

    [Header("Element references:")]
    [SerializeField] TextMeshProUGUI[] headerTexts;
    [SerializeField] TextMeshProUGUI[] infoTexts;
    [SerializeField] TextMeshProUGUI[] infoInvertedTexts;
    [SerializeField] Image[] uiBGImages;
    [SerializeField] Image[] uiHeaderImages;
    [SerializeField] Image[] uiTopBarImages;

    Color buttonFGOld;
    Color buttonBGOld;
    Color buttonHighlightOld;
    Color uiBaseOld;
    Color depthMeterOld;
    Color playerOld;
    Color enviornmentOld;
    Color pingOld;
    bool recordedColors;


    [SerializeField] ColorPallette[] styles;
    int currentStyleIndex = 0;

    private void Awake()
    {
        keepChangesAfterGameMode = false;

    }

    [ContextMenu("Test new Style.")]
    public void TestNewStyle()
    {
        if (!keepChangesAfterGameMode && !recordedColors)
        {
            buttonFGOld = buttonFG.GetColor("_Color");
            buttonBGOld = buttonBG.GetColor("_Color");
            buttonHighlightOld = buttonHighlight.GetColor("_Color");
            uiBaseOld = uiBase.GetColor("_Color");
            depthMeterOld = depthMeter.GetColor("_Color");
            playerOld = player.GetColor("_Color");
            enviornmentOld=enviornment.GetColor("_Color");
            pingOld=ping.GetColor("_Color");

            recordedColors = true;
        }

        buttonFG.SetColor("_Color", buttonFGColor);
        buttonBG.SetColor("_Color", buttonBGColor);
        buttonHighlight.SetColor("_Color", buttonHighlightColor);
        uiBase.SetColor("_Color", uiBaseColor);
        depthMeter.SetColor("_Color", depthMeterColor);
        player.SetColor("_Color", playerColor);
        playerCollision.SetColor("_Color", playerColor);
        enviornment.SetColor("_Color", enviornmentColor);
        ping.SetColor("_Color", pingColor);

        foreach (var item in uiBGImages)
        {
            item.color = uiBaseColorBG;
        }
        foreach (var item in headerTexts)
        {
            item.color = textHeaderColor;
        }
        foreach (var item in infoTexts)
        {
            item.color = textInfoColor;
        }
        foreach (var item in infoInvertedTexts)
        {
            item.color = uiBaseColorBG;
        }
        foreach (var item in uiHeaderImages)
        {
            item.color = uiHeaderColor;
        }
        foreach (var item in uiTopBarImages)
        {
            item.color = buttonFGColor;
        }
    }
    [ContextMenu("RevertMaterialChanges")]
    public void RevertMaterialColors()
    {
        if (!recordedColors) return;
        buttonFG.SetColor("_Color", buttonFGOld);
        buttonBG.SetColor("_Color", buttonBGOld);
        buttonHighlight.SetColor("_Color", buttonHighlightOld);
        uiBase.SetColor("_Color", uiBaseOld);
        depthMeter.SetColor("_Color", depthMeterOld);
        player.SetColor("_Color", playerOld);
        playerCollision.SetColor("_Color", playerOld);
        enviornment.SetColor("_Color", enviornmentOld);
        ping.SetColor("_Color", pingOld);
        recordedColors = false;
    }
    private void OnDisable()
    {
        if(!keepChangesAfterGameMode) { RevertMaterialColors(); }
    }

    private void OnValidate()
    {
        TestNewStyle();

    }
}
