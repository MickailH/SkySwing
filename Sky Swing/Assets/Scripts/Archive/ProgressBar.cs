using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxProgress(float Progress)
    {
        slider.maxValue = Progress;
        slider.value = Progress;
    }

    public void SetProgress(float Progress)
    {
        slider.value = Progress;
    }
}
