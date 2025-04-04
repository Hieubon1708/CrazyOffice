using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Bot
{
    protected int hp = 6;
    protected int startHp;

    public Transform neck;

    protected BossHealth bossHealth;
    public GameObject healthBar;

    public Transform targetStrangle;

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

    public abstract void SubtractHp();

    public void Strangle()
    {
        navMeshAgent.enabled = false;
        transform.SetParent(PlayerController.instance.transform);

        animator.SetTrigger("Strangle");
    }

    public void HitFirst()
    {
        animator.SetTrigger("HitFirst");
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.SeeBoss();
        }
    }
}
