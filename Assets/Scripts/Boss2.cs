using DG.Tweening;
using System.Collections;
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

    public GameObject bone;

    public void Start()
    {
        hp = 50;
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

        if (hp % 5 == 0)
        {
            materials[1].SetTexture("_OverlayTex", GameController.instance.bossFaces[GetIndexTex()]);
            materials[1].SetFloat("_HasOverlayTexture", 1);
            indexTex--;
        }

        AudioController.instance.PlayVibrate(50);

        base.SubtractHp();

        if (hp == 0)
        {
            isElectric = false;

            fxElectric.Stop();

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.endElectricity, 50);

            PlayerController.instance.isSoloBoss = false;

            AudioController.instance.StopSrcLoop();

            gameObject.SetActive(false);
            bone.SetActive(true);

            DOVirtual.DelayedCall(2.5f, delegate
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

                    PlayerController.instance.Kick();
                }
            }
        }
    }

    public IEnumerator LockChair()
    {
        yield return new WaitForSeconds(0.25f);

        lockChairRight.DOLocalRotate(Vector3.zero, 0.25f).SetUpdate(true);
        lockChairLeft.DOLocalRotate(Vector3.zero, 0.25f);
        lockChairCap.DOLocalMoveY(0, 0.25f).SetDelay(0.5f).SetUpdate(true);

        isAfterLockChair = true;

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

    public void EletricShock()
    {
        if (hand.activeSelf) hand.SetActive(false);

        handle.DORotate(new Vector3(180f, 0f, 0f), 0.25f, RotateMode.LocalAxisAdd).OnComplete(delegate
        {
            AudioController.instance.PlaySoundNVibrateLoop(AudioController.instance.electricity, 0);

            isElectric = true;

            fxElectric.Play();

            animator.SetTrigger("ElectricShock");

            PlayerController.instance.CompletelyAttack();

        }).SetUpdate(true);
    }

    public void Kick()
    {
        navMeshAgent.enabled = false;

        animator.SetTrigger("KickFront");

        transform.SetParent(chairReal);

        transform.DOLocalMove(new Vector3(0f, 0.035f, 0f), 0.25f).SetUpdate(true);
        transform.DOLocalRotate(Vector3.zero, 0.25f).SetUpdate(true).OnComplete(delegate
        {
            animator.SetTrigger("Sit");
            StartCoroutine(LockChair());
        });
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
