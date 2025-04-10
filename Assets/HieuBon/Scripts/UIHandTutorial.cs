using DG.Tweening;
using UnityEngine;

public class UIHandTutorial : MonoBehaviour
{
    public RectTransform hand;
    public RectTransform[] path;
    public CanvasGroup canvasGroup;
    Vector3[] paths;

    public void PlayHand()
    {
        paths = new Vector3[path.Length];
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = path[i].position;
        }

        hand.position = paths[0];
        hand.DOPath(paths, 3f, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
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
}
