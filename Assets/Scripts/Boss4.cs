using DG.Tweening;
using System.Collections;
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

    public SkinnedMeshRenderer MeshFilter;

    public void Start()
    {
        hp = 100;
        startHp = hp;

        Mesh mesh = MeshFilter.sharedMesh;
        Vector2[] uvs = mesh.uv;

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = Vector2.zero;
        }
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

        base.SubtractHp();

        if (hp == 0)
        {
            PlayerController.instance.isSoloBoss = false;

            HeadDipExit();

            animator.SetTrigger("DieByHeadDip");

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

                    DOVirtual.DelayedCall(0.25f, delegate
                    {
                        PlayerController.instance.foot.PlayAnimationKickRight();
                    });
                }
            }
        }
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

        AudioController.instance.StopSrcLoop();
        AudioController.instance.srcLoop.clip = null;

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

    public void AfterKickRight()
    {
        navMeshAgent.enabled = false;

        animator.SetTrigger("KickRight");

        transform.DOMove(pot.position, 1f).SetUpdate(true).OnComplete(delegate
        {
            StartCoroutine(AfterDropDown());
        });

        Vector3 dir = pot.position - transform.position;
        Quaternion lookAt = Quaternion.LookRotation(dir);

        transform.DORotateQuaternion(lookAt, 1f).SetUpdate(true);
    }

    public IEnumerator AfterDropDown()
    {
        yield return new WaitForSeconds(0.75f);

        isAfterHeadDip = true;

        PlayerController.instance.navMeshAgent.angularSpeed = 0;

        PlayerController.instance.ResumeMove();

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

                if (hp == 0) return;

                if (potCam.depth == 1)
                {
                    potCam.transform.DOShakeRotation(0.1f, 2.5f, 1).SetUpdate(true);
                    if (AudioController.instance.srcLoop.clip != AudioController.instance.balloon) AudioController.instance.PlaySoundNVibrateLoop(AudioController.instance.balloon, 0);
                }
                else
                {
                    if (AudioController.instance.srcLoop.clip != AudioController.instance.waterSplash) AudioController.instance.PlaySoundNVibrateLoop(AudioController.instance.waterSplash, 0);
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
