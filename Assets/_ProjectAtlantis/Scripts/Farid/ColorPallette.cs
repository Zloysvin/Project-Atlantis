using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPallette", menuName = "Scriptable Objects/ColorPallette")]
public class ColorPallette : ScriptableObject
{
    [Header("Fonts:")]
    [SerializeField] public TMP_FontAsset PrimaryFont;
    [SerializeField] public TMP_FontAsset SecondaryFont;
    [Header("Colors:")]
    [SerializeField] public Color PrimaryColor = Color.white;
    [SerializeField] public Color SecondaryColor = Color.black;
    [SerializeField] public Color TertiaryColor = Color.blue;
    [SerializeField] public Color QuadColor = Color.green;
}
