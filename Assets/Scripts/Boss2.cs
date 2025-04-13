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

    public ParticleSystem fxElectric;

    bool isElectric;
    public void Start()
    {
        hp = 25;
        startHp = hp;
    }

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
            isElectric = false;

            fxElectric.Stop();

            PlayerController.instance.isSoloBoss = false;

            DOVirtual.DelayedCall(1f, delegate
            {
                PlayerController.instance.Move();
            });
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

        yield return new WaitUntil(() => isAfterLockChair);

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

        handle.DORotate(new Vector3(180f, 0f, 0f), 0.25f, RotateMode.LocalAxisAdd).OnComplete(delegate
        {
            isElectric = true;

            fxElectric.Play();

            animator.SetTrigger("ElectricShock");
        }).SetUpdate(true);

        PlayerController.instance.CompletelyAttack();
    }

    float time;
    float totalTime;

    public void Update()
    {
        if (isElectric)
        {
            totalTime += Time.deltaTime;

            if (totalTime >= time)
            {
                time += 0.1f;

                SubtractHp();

                PlayerController.instance.transform.DOShakeRotation(0.1f, 2.5f, 1).SetUpdate(true);
            }
        }
    }
}
