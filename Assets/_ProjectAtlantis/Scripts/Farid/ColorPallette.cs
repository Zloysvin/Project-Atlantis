using UnityEngine;

[CreateAssetMenu(fileName = "ColorPallette", menuName = "Scriptable Objects/ColorPallette")]
public class ColorPallette : ScriptableObject
{
    [Header("New Colors:")]
    [SerializeField] public Color buttonFGColor = Color.green;

    [SerializeField] public Color buttonBGColor = Color.black;
    [SerializeField] public Color buttonHighlightColor = Color.yellow;
    [SerializeField] public Color uiBaseColor = Color.blue;
    [SerializeField] public Color uiBaseColorBG = Color.black;
    [SerializeField] public Color uiHeaderColor = Color.green;
    [SerializeField] public Color depthMeterColor = Color.green;
    [SerializeField] public Color playerColor = Color.green;
    [SerializeField] public Color enviornmentColor = Color.blue;
    [SerializeField] public Color pingColor = Color.green;
    [SerializeField] public Color textHeaderColor = Color.green;
    [SerializeField] public Color textInfoColor = Color.green;
}