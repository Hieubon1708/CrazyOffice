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
        mask.DOFillAmount((float)percent / 100f, 1f).OnComplete(delegate
        {
            if (callback != null) callback.Invoke();
        });

        DOVirtual.Int(percent - 25, percent, 1f, (v) =>
        {
            textPercent.text = v.ToString() + "%";
        });
    }

    public void LoadData(int percent)
    {
        mask.fillAmount = (float)percent / 100f - 0.25f;

        Debug.Log(mask.fillAmount);
        textPercent.text = (percent - 25).ToString() + "%";
    }
}
