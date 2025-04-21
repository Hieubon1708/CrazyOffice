using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Boss3 : Boss
{
    public Transform torture;

    public GameObject hand;

    public Transform handPosition;

    public Transform woodBar;

    public Transform[] afterStrangle;

    public Transform pointAfterKneel;
    public Transform lookAtAfterKneel;

    public Transform knife;

    bool isAfterKneel;

    public Rigidbody headCut;

    public GameObject[] necks;

    public ParticleSystem blood;

    public Transform targetFoot;

    public override Vector3 TargetPosition
    {
        get
        {
            if (isAfterKneel)
            {
                return pointAfterKneel.position;
            }

            if (navMeshAgent.enabled) return transform.position;
            return torture.position;
        }
    }

    public override Vector3 TargetRotation
    {
        get
        {
            if (isAfterKneel)
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

        AudioController.instance.PlayVibrate(50);

        base.SubtractHp();

        blood.Play();

        if (hp == 0)
        {
            foreach (GameObject n in necks)
            {
                n.transform.localScale = Vector3.zero;
            }

            headCut.gameObject.SetActive(true);
            headCut.AddForce(new Vector3(0, 10, 1), ForceMode.Impulse);

            PlayerController.instance.isSoloBoss = false;

            DOVirtual.DelayedCall(1.5f, delegate
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

                    BeforeKneelDown();
                }
            }
        }
    }

    public void BeforeKneelDown()
    {
        Hand hand = PlayerController.instance.hand;

        hand.handPivot.transform.SetParent(targetHandOrFoot);

        hand.gameObject.SetActive(true);

        hand.handPivot.transform.position = afterStrangle[1].position;

        hand.handPivot.transform.DOLocalRotateQuaternion(Quaternion.Euler(2.448f, 33.371f, -22.854f), 0.35f).SetUpdate(true);
        hand.handPivot.transform.DOMove(afterStrangle[0].position, 0.35f).SetUpdate(true).OnComplete(delegate
        {
            transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f, RotateMode.WorldAxisAdd).SetUpdate(true).SetDelay(0.15f).OnComplete(delegate
            {
                hand.handPivot.gameObject.SetActive(false);

                PlayerController.instance.Kick();
            });
        });
    }

    public void Kick()
    {
        navMeshAgent.enabled = false;

        animator.SetTrigger("KickBehind");

        transform.DOMove(torture.position, 0.25f).SetUpdate(true).OnComplete(delegate
        {
            animator.SetTrigger("Kneel");
            StartCoroutine(AfterKneel());
        });
    }

    public void Cut()
    {
        if (hand.activeSelf) hand.SetActive(false);

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.behead, 50);

        knife.DOLocalMoveY(0.15f, 0.05f).OnComplete(delegate
        {
            animator.SetTrigger("Cut");
            SubtractHp();
            PlayerController.instance.CompletelyAttack();
            PlayerController.instance.transform.DOShakeRotation(0.25f, 5f, 50).SetUpdate(true);
            knife.DOLocalMoveY(1.070468f, 0.25f).SetUpdate(true);
        }).SetUpdate(true);
    }

    public void Strangle()
    {
        navMeshAgent.enabled = false;
        transform.SetParent(PlayerController.instance.transform);

        animator.SetTrigger("Strangle");
    }

    public IEnumerator AfterKneel()
    {
        yield return new WaitForSeconds(0.35f);

        transform.SetParent(GameController.instance.levelObject.transform);

        woodBar.DOLocalMoveY(0.3312885f, 0.25f).SetUpdate(true).OnComplete(delegate
        {
            isAfterKneel = true;

            PlayerController.instance.navMeshAgent.angularSpeed = 0;

            PlayerController.instance.ResumeMove();

            PlayerController.instance.tRotate = 0.025f;
            PlayerController.instance.isRoting = true;
        });

        yield return new WaitUntil(() => isAfterKneel);

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => PlayerController.instance.navMeshAgent.remainingDistance == 0);

        hand.transform.position = Camera.main.WorldToScreenPoint(handPosition.position);

        hand.SetActive(true);

        healthBar.SetActive(true);

        PlayerController.instance.isSoloBoss = true;
    }
}
