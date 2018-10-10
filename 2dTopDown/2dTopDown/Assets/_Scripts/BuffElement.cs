using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffElement : MonoBehaviour
{
    public Image Image;
    public Image DurationImage;
    public Text DurationText;

    public bool Active;

    private void Awake()
    {
        DurationText.text = "";
        DurationImage.fillAmount = 1f;
    }

    public void Activate(BuffDescriptor buffDescriptor)
    {
        Active = true;
        gameObject.SetActive(true);
        Image.sprite = buffDescriptor.Sprite;
        DurationImage.fillAmount = 0;
        DurationText.text = buffDescriptor.Duration.ToString("n1");
        transform.SetAsFirstSibling();
    }

    public void Refresh(float Duration, float FullDuration)
    {
        DurationText.text = Duration.ToString("n1");
        DurationImage.fillAmount = Duration / FullDuration;
    }


    internal void Deactivate()
    {
        Active = true;
        gameObject.SetActive(false);
    }
}
