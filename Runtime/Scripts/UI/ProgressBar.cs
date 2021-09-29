using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image FillBar;

    public void SetValue(float value)
    {
        FillBar.fillAmount = value;
    }
}
