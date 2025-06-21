using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Mask mask;
    float originalSize;
    public static UIHealthBar instance { get; private set; }
    private void Awake()
    {
        instance = this;
        originalSize = mask.rectTransform.rect.width;
    }
    // void Start()
    // {
        
    // }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
