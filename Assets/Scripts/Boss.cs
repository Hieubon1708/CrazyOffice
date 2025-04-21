using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Bot
{
    protected int hp = 7;
    protected int startHp;

    public Transform neck;

    protected BossHealth bossHealth;
    public GameObject healthBar;

    public Transform targetHandOrFoot;

    [HideInInspector]
    public Vector3 headStatic;

    public override Vector3 TargetRotation
    {
        get
        {
            if (navMeshAgent.enabled) return rbs[0].position;
            return transform.position;
        }
    }

    public override void Awake()
    {
        base.Awake();

        bossHealth = GetComponentInChildren<BossHealth>(true);

        startHp = hp;

        healthBar.SetActive(false);
    }

    public virtual void SubtractHp()
    {
        hp -= 1;
        bossHealth.SubtractHp(startHp, startHp - hp);

        if (hp == 0)
        {
            UIController.instance.totalEarn += Random.Range(50, 60);
        }
    }

    public void HitFirst()
    {
        animator.SetTrigger("HitFirst");
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerController.instance.SeeBoss());
        }
    }
}
