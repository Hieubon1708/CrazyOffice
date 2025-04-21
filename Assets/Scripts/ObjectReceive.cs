using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectReceive : MonoBehaviour
{
    public Image mask;
    public TextMeshProUGUI textPercent;

    public void Increase(int percent, Action callback)
    {
        mask.DOFillAmount((float)percent / 100f, 1.25f).OnComplete(delegate
        {
            if (callback != null) callback.Invoke();
        });

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.increasePercent, 1250);

        DOVirtual.Int(percent - 25, percent, 1.25f, (v) =>
        {
            textPercent.text = v.ToString() + "%";
        }).OnComplete(delegate
        {
            if(percent == 100)
            {
                AudioController.instance.PlaySoundNVibrate(AudioController.instance.completePercent, 100);
            }
        });
    }

    public void LoadData(int percent)
    {
        if(percent == 0)
        {
            mask.fillAmount = 0;
            textPercent.text = "";
        }
        else
        {
            mask.fillAmount = (float)percent / 100f - 0.25f;

            textPercent.text = (percent - 25).ToString() + "%";
        }
    }

    public void IsActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
