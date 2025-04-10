using DG.Tweening;
using RootMotion.Dynamics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Boss4 : Boss
{
    public Transform pot;
    public Transform potReal;

    public Camera potCam;

    public GameObject hand;

    public Transform handPosition;

    public Transform[] afterStrangle;

    public Transform pointAfterKneel;
    public Transform lookAtAfterKneel;

    public ParticleSystem fxWater;

    public GameObject uIWater;

    public Image layerFade;

    bool isAfterHeadDip;
    bool isInWater;

    public void Start()
    {
        hp = 100;
        startHp = hp;
    }

    public override Vector3 TargetPosition
    {
        get
        {
            if (isAfterHeadDip)
            {
                return pointAfterKneel.position;
            }

            if (navMeshAgent.enabled) return transform.position;
            return pot.position;
        }
    }

    public override Vector3 TargetRotation
    {
        get
        {
            if (isAfterHeadDip)
            {
                return lookAtAfterKneel.position;
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

            HeadDipExit();

            animator.SetTrigger("DieByHeadDip");
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

    public void DropDown(Transform hand)
    {
        hand.gameObject.SetActive(false);

        animator.SetTrigger("Idle");

        transform.DOMoveY(transform.position.y - 1f, 0.25f).OnComplete(delegate
        {
            DOVirtual.DelayedCall(0.5f, delegate
            {
                hand.gameObject.SetActive(true);

                hand.position = afterStrangle[1].position;

                hand.DOLocalRotateQuaternion(Quaternion.Euler(2.448f, 33.371f, -22.854f), 0.35f).SetUpdate(true);
                hand.DOMove(afterStrangle[0].position, 0.35f).SetUpdate(true).OnComplete(delegate
                {
                    transform.DOLookAt(potReal.position, 0.5f, AxisConstraint.Y).SetUpdate(true).SetDelay(0.15f).OnComplete(delegate
                    {
                        StartCoroutine(AfterDropDown());
                    });
                });
            }).SetUpdate(true);
        }).SetUpdate(true);
    }

    public void HeadDip()
    {
        if (this.hand.activeSelf) this.hand.SetActive(false);

        Transform hand = PlayerController.instance.hand.handPivot.transform;

        hand.gameObject.SetActive(true);

        animator.SetBool("HeadDip", true);
    }

    public void AfterHeadDip()
    {
        if (animator.GetBool("HeadDip")) isInWater = true;
    }

    public void HeadDipExit()
    {
        animator.SetBool("HeadDip", false);

        Transform hand = PlayerController.instance.hand.handPivot.transform;

        hand.gameObject.SetActive(false);

        isInWater = false;

        layerFade.DOKill();
        Color color = layerFade.color;
        color.a = 0;
        layerFade.color = color;

        uIWater.SetActive(false);
        potCam.depth = -1f;
        PlayerController.instance.CompletelyAttack();

        time = 0f;
        totalTime = 0;
    }

    public IEnumerator AfterDropDown()
    {
        transform.SetParent(GameController.instance.levelObject.transform);

        PlayerController.instance.hand.handPivot.SetActive(false);

        isAfterHeadDip = true;

        PlayerController.instance.navMeshAgent.angularSpeed = 0;

        PlayerController.instance.ResumeMove();

        PlayerController.instance.tRotate = 0.025f;
        PlayerController.instance.isRoting = true;

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => PlayerController.instance.navMeshAgent.remainingDistance == 0);

        hand.transform.position = Camera.main.WorldToScreenPoint(handPosition.position);

        hand.SetActive(true);

        healthBar.SetActive(true);

        PlayerController.instance.isSoloBoss = true;
    }

    float time;
    float totalTime;

    public void Update()
    {
        if (isInWater)
        {
            totalTime += Time.deltaTime;

            if (totalTime >= time)
            {
                time += 0.1f;

                SubtractHp();

                if (potCam.depth == 1)
                {
                    potCam.transform.DOShakeRotation(0.1f, 2.5f, 1).SetUpdate(true);
                }
                else
                {
                    PlayerController.instance.transform.DOShakeRotation(0.1f, 2.5f, 1).SetUpdate(true);
                }

                fxWater.Play();

                if (time >= 1 && layerFade.color.a == 0 && potCam.depth == -1)
                {
                    layerFade.DOFade(1f, 0.1f).OnComplete(delegate
                    {
                        potCam.depth = 1f;
                        uIWater.SetActive(true);
                        layerFade.DOFade(0f, 0.1f).SetUpdate(true).SetEase(Ease.Linear);
                    }).SetUpdate(true).SetEase(Ease.Linear);
                }
            }
        }
    }
}
