using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PanelLose : MonoBehaviour
{
    public CanvasGroup panelLose;

    bool isTry;

    [HideInInspector]
    public bool isTweening;

    public void Show()
    {
        isTry = false;

        isTweening = true;

        gameObject.SetActive(true);

        panelLose.alpha = 0f;

        panelLose.DOFade(1f, 0.5f).OnComplete(delegate
        {
            isTweening = false;
        });
    }

    public void Hide()
    {
        if (isTry) return;

        isTry = true;

        isTweening = true;

        panelLose.DOFade(0f, 0.5f).OnComplete(delegate
        {
            gameObject.SetActive(false);

            isTweening = true;
        });
    }
}
