using DG.Tweening;
using System;
using UnityEngine;

public class UIHandTutorial : MonoBehaviour
{
    public RectTransform hand;
    public RectTransform[] path;
    public CanvasGroup canvasGroup;
    Vector3[] paths;

    private void Awake()
    {
        DOVirtual.DelayedCall(0.5f, delegate
        {
            CreatePath();
        });
    }

    void CreatePath()
    {
        paths = new Vector3[path.Length];
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = path[i].position;
        }
    }

    public void PlayHand()
    {
        Action action = () =>
        {
            hand.position = paths[0];
            hand.DOPath(paths, 3f, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        };

        if (paths == null || paths.Length == 0)
        {
            DOVirtual.DelayedCall(0.5f, delegate
            {
                CreatePath();
                action.Invoke();
            });
        }
        else
        {
            action.Invoke();
        }
    }

    public void Hide()
    {
        if (canvasGroup.alpha == 0) return;

        hand.DOKill();
        canvasGroup.DOKill();

        canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        PlayHand();
        canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        Hide();
    }
}
