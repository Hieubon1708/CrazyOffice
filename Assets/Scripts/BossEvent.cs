using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    public Transform parent;
    public AnimationClip clip;

    public void AfterThrowWeapon()
    {
        PlayerController.instance.ResumeMove();
    }

    public void Rotate()
    {
        parent.DOLocalRotate(new Vector3(0, 180, 0), clip.length).SetUpdate(true).OnComplete(delegate
        {
            Boss boss = PlayerController.instance.CurrentBoss;

            if(boss is Boss1)
            {
                (boss as Boss1).SetHeadStatic();
            }
        }).SetUpdate(true);
    }
}
