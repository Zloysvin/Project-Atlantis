using UnityEngine;
using UnityEngine.U2D;

public class ChangeMaskMode : MonoBehaviour
{
    [SerializeField] private SpriteMaskInteraction maskMode = SpriteMaskInteraction.VisibleInsideMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.maskInteraction = maskMode;
        }
        else
        {
            SpriteShapeRenderer shapeRenderer = GetComponent<SpriteShapeRenderer>();
            if (shapeRenderer != null)
            {
                shapeRenderer.maskInteraction = maskMode;
            }
        }
    }
}