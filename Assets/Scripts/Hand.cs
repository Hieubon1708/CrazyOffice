using DG.Tweening;
using System.Collections;
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
        gameObject.SetActive(true);

        animator.Rebind();

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        handPivot.transform.position = PlayerController.instance.transform.position;
        handPivot.transform.localRotation = Quaternion.Euler(new Vector3(-18.183f, 26.289f, -44.006f));

        Boss boss = PlayerController.instance.CurrentBoss;

        handPivot.transform.DOMove(boss.targetHandOrFoot.position, 0.5f).OnComplete(delegate
        {
            handPivot.transform.SetParent(boss.targetHandOrFoot);
            if(boss is Boss1) (boss as Boss1).Strangle();
            if(boss is Boss3) (boss as Boss3).Strangle();

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
        }
    }
}
