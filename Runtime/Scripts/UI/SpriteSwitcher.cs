using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite[] Sprites;

    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private int spriteNowIndex = 0;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();
    }

    public void SetNextSprite()
    {
        spriteNowIndex++;
        if (Sprites.Length == spriteNowIndex)
            spriteNowIndex = 0;

        if (_spriteRenderer)
            _spriteRenderer.sprite = Sprites[spriteNowIndex];
        if (_image)
            _image.sprite = Sprites[spriteNowIndex];
        
        SetSpriteByIndex(spriteNowIndex);
    }
    
    public void SetPreviousSprite()
    {
        spriteNowIndex--;
        if (spriteNowIndex < 0)
            spriteNowIndex = Sprites.Length - 1;
        
        SetSpriteByIndex(spriteNowIndex);
    }

    public void SetSpriteByIndex(int index)
    {
        if (index >= Sprites.Length)
            index = Sprites.Length - 1;
        else if (index < 0)
            index = 0;
        
        if (_spriteRenderer)
            _spriteRenderer.sprite = Sprites[index];
        if (_image)
            _image.sprite = Sprites[index];
    }
}
