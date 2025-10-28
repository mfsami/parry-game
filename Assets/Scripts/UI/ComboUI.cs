using System.Collections.Generic;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    public Sprite upArrowSprite, downArrowSprite, leftArrowSprite, rightArrowSprite;
    
    public Player Player;

    Dictionary<KeyCode, Sprite> keySprites;

    [SerializeField] SpriteRenderer[] arrowSlots;

    private void Start()
    {
        // Start with no sprites shown
        HideCombo();
    }

    void Awake()
    {
        keySprites = new Dictionary<KeyCode, Sprite>()
        {
            { KeyCode.UpArrow, upArrowSprite },
            { KeyCode.DownArrow, downArrowSprite },
            { KeyCode.LeftArrow, leftArrowSprite },
            { KeyCode.RightArrow, rightArrowSprite }
        };
    }

    public void ShowCombo(List<KeyCode> combo)
    {
        // Show combo
        foreach(var slot in arrowSlots)
        {
            slot.enabled = true;
        }

        // Loop through combo and set sprites
        for (int i = 0; i < arrowSlots.Length; i++)
        {
            if (i < combo.Count)
            {
                arrowSlots[i].sprite = keySprites[combo[i]];
            }
            else
            {
                arrowSlots[i].sprite = null; // clear slot
            }
        }
    }

    public void HideCombo()
    {
        foreach (var slot in arrowSlots)
        {
            slot.enabled = false;
        }
    }

    
}
