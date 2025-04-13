using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Animator animator;

    public GameObject handPivot;

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>(true);
    }

    public void Slap(Vector3 position, Vector3 lookAt, float angle, bool isRight)
    {
        transform.position = position;
        transform.LookAt(lookAt);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        transform.localScale = new Vector3(isRight ? 1f : -1f, 1f, 1f);

        animator.SetTrigger("Slap");
    }

    public void Strangle()
    {
        animator.Rebind();

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        handPivot.transform.position = PlayerController.instance.transform.position;
        handPivot.transform.localRotation = Quaternion.Euler(new Vector3(-18.183f, 26.289f, -44.006f));

        Boss boss = PlayerController.instance.CurrentBoss;

        handPivot.transform.DOMove(boss.targetStrangle.position, 0.5f).OnComplete(delegate
        {
            handPivot.transform.SetParent(boss.targetStrangle);
            boss.Strangle();

            boss.transform.DOMoveY(boss.transform.position.y + 1f, 1f).OnComplete(delegate
            {
                StartCoroutine(MoveToNextPoint());
            }).SetUpdate(true);
        }).SetUpdate(true);
    }

    IEnumerator MoveToNextPoint()
    {
        PlayerController.instance.ResetParam();
        PlayerController.instance.ResumeMove();

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => PlayerController.instance.navMeshAgent.remainingDistance == 0);
        yield return new WaitForSeconds(0.5f);

        Boss boss = PlayerController.instance.CurrentBoss;

        switch (boss)
        {
            case Boss1:

                Boss1 boss1 = (Boss1)boss;

                boss1.DropToilet();

                handPivot.SetActive(false);

                break;

            case Boss2:

                Boss2 boss2 = (Boss2)boss;

                boss2.Sit();

                handPivot.SetActive(false);

                break;

            case Boss3:

                Boss3 boss3 = (Boss3)boss;

                boss3.KneelDown(handPivot.transform);

                break;

            case Boss4:

                Boss4 boss4 = (Boss4)boss;

                boss4.DropDown(handPivot.transform);

                break;
        }
    }
}
