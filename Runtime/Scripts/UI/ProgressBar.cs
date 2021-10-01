using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image FillBar;
    public RectTransform MovedObject;

    
    
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetValue(float value)
    {
        FillBar.fillAmount = value;
        
        if(MovedObject && rectTransform)
            MovedObject.anchoredPosition = new Vector2(rectTransform.rect.width * FillBar.fillAmount, 0);
    }
}
