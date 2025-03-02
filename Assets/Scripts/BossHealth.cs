using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    Image bar;

    private void Awake()
    {
        bar = GetComponent<Image>();
    }

    public void SubtractHp(int startHp, int currentHp)
    {
        bar.DOFillAmount((float)currentHp / startHp, 0.25f);
    }
}
