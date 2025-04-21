using DG.Tweening;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public GameObject footPivot;

    public Animation ani;

    private void Awake()
    {
        ani = GetComponentInChildren<Animation>(true);
    }

    public void Kick()
    {
        gameObject.SetActive(true);

        Boss boss = PlayerController.instance.CurrentBoss;

        footPivot.transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.5f).SetUpdate(true);
        footPivot.transform.DOLocalMove(new Vector3(0, 1.65f, 1.65f), 0.5f).OnComplete(delegate
        {
            footPivot.transform.DOMove(boss.targetHandOrFoot.position, 0.25f).SetUpdate(true).SetDelay(0.5f).SetEase(Ease.InBack).OnComplete(delegate
            {
                footPivot.SetActive(false);

                if(boss is Boss2) (boss as Boss2).Kick();
                if(boss is Boss3) (boss as Boss3).Kick();
            });
        }).SetUpdate(true);
    }
    
    public void PlayAnimationKickRight()
    {
        gameObject.SetActive(true);

        ani.Play();
    }
}
