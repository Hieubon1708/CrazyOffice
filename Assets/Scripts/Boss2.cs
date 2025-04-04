using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2 : Boss
{
    public Transform chair;
    public Transform chairReal;

    public Transform lockChairRight;
    public Transform lockChairLeft;
    public Transform lockChairCap;

    bool isAfterLockChair;

    public Transform pointAfterLockChair;
    public Transform lookAtAfterChair;

    public GameObject hand;

    public Transform handPosition;

    public Transform handle;

    public override Vector3 TargetPosition
    {
        get
        {
            if (isAfterLockChair)
            {
                return pointAfterLockChair.position;
            }

            if (navMeshAgent.enabled) return transform.position;
            return chair.position;
        }
    }

    public override Vector3 TargetRotation
    {
        get
        {
            if (isAfterLockChair)
            {
                return lookAtAfterChair.position;
            }

            if (navMeshAgent.enabled) return rbs[0].position;
            return transform.position;
        }
    }

    public override void SubtractHp()
    {
        if (hp <= 0) return;
        hp -= 1;
        bossHealth.SubtractHp(startHp, startHp - hp);

        if (hp == 0)
        {
            PlayerController.instance.isSoloBoss = false;

            healthBar.SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        if (PlayerController.instance != null && navMeshAgent.enabled)
        {
            if (isTarget)
            {
                Vector3 target = PlayerController.instance.transform.position;

                float distance = Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z));

                if (distance <= GameController.instance.distanceToKill)
                {
                    isTarget = false;

                    PlayerController.instance.StopMove();

                    PlayerController.instance.Strangle();
                }
            }
        }
    }

    public void Sit()
    {
        transform.SetParent(chairReal);

        animator.SetTrigger("Sit");

        transform.DOLocalMove(new Vector3(0f, 0.035f, 0f), 0.25f).SetUpdate(true);
        transform.DOLocalRotate(Vector3.zero, 0.25f).SetUpdate(true);
        DOVirtual.DelayedCall(1f, delegate
        {
            StartCoroutine(LockChair());
        }).SetUpdate(true);

    }
    public IEnumerator LockChair()
    {
        lockChairRight.DOLocalRotate(Vector3.zero, 0.25f).SetUpdate(true);
        lockChairLeft.DOLocalRotate(Vector3.zero, 0.25f).OnComplete(delegate
        {
            lockChairCap.DOLocalMoveY(0, 0.25f).SetDelay(0.5f).SetUpdate(true);
        }).SetUpdate(true);

        DOVirtual.DelayedCall(1.5f, delegate
        {
            isAfterLockChair = true;

            PlayerController.instance.navMeshAgent.angularSpeed = 0;

            PlayerController.instance.ResumeMove();

            PlayerController.instance.tRotate = 0.025f;
            PlayerController.instance.isRoting = true;
        }).SetUpdate(true);

        yield return new WaitForSeconds(1.5f);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => PlayerController.instance.navMeshAgent.remainingDistance == 0);

        hand.transform.position = Camera.main.WorldToScreenPoint(handPosition.position);

        hand.SetActive(true);

        healthBar.SetActive(true);

        PlayerController.instance.isSoloBoss = true;
    }

    public void EletricShock()
    {
        if (hand.activeSelf) hand.SetActive(false);

        int step = 0;

        int loop = hp == 1 ? 1 : 2;

        handle.DORotate(new Vector3(180f, 0f, 0f), 0.25f, RotateMode.LocalAxisAdd).SetLoops(loop, LoopType.Yoyo).OnStepComplete(delegate
        {
            if (step == 0)
            {
                PlayerController.instance.isRoting = false;

                animator.SetTrigger("ElectricShock");
                SubtractHp();
                PlayerController.instance.CompletelyAttack();
                PlayerController.instance.transform.DOShakeRotation(0.25f, 5f, 50).SetUpdate(true);
            }
            step++;
        }).SetUpdate(true);
    }
}
