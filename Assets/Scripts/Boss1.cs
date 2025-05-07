using DG.Tweening;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.AI;

public class Boss1 : Boss
{
    public Transform toilet;
    public Transform toiletHole;

    public UIHandTutorial hand;
    public ParticleSystem toiletPar;

    public override Vector3 TargetPosition
    {
        get
        {
            if (navMeshAgent.enabled) return transform.position;
            return toilet.position;
        }
    }

    public override void SubtractHp()
    {
        if (hp <= 0) return;

        materials[1].SetTexture("_OverlayTex", GameController.instance.bossFaces[GetIndexTex()]);
        materials[1].SetFloat("_HasOverlayTexture", 1);
        indexTex--;

        base.SubtractHp();

        if (hp == 0)
        {
            PlayerController.instance.isSoloBoss = false;

            DOVirtual.DelayedCall(1f, delegate
            {
                PlayerController.instance.Strangle();
            }).SetUpdate(true);
        }
    }

    public void Hit(Vector2 dir)
    {
        animator.SetFloat("Blend_X", dir.x);
        animator.SetFloat("Blend_Y", dir.y);

        animator.SetTrigger("Hit");

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.slaps, 50);

        PlayerController.instance.transform.DOShakeRotation(0.25f, 5f, 25).SetUpdate(true);

        SubtractHp();
    }

    public void Strangle()
    {
        navMeshAgent.enabled = false;
        transform.SetParent(PlayerController.instance.transform);

        animator.SetTrigger("Strangle");
    }

    public void SetHeadStatic()
    {
        headStatic = head.position;
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

                    healthBar.SetActive(true);

                    PlayerController.instance.StopMove();
                    PlayerController.instance.isSoloBoss = true;

                    hand.Show();

                    PlayerController.instance.hand.gameObject.SetActive(true);
                }
            }
        }
    }

    public void DropToilet()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.flushToilet, 0);
        toiletPar.Play();
        healthBar.SetActive(false);
        transform.DOMove(toiletHole.position, 1.5f).SetUpdate(true);
        transform.DOScale(0f, 1.5f).SetUpdate(true).OnComplete(delegate
        {
            PlayerController.instance.Move();
        });
    }
}
