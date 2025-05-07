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

    SkinnedMeshRenderer meshRenderer;

    protected Material[] materials;

    protected int indexTex = -1;

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

        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        materials = meshRenderer.sharedMaterials;
        materials[1].SetFloat("_HasOverlayTexture", 0);

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

    protected int GetIndexTex()
    {
        if (indexTex == -1) indexTex = GameController.instance.bossFaces.Length - 1;
        return indexTex;
    }

    public void HitFirst()
    {
        animator.SetTrigger("HitFirst");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(PlayerController.instance.SeeBoss());
        }
    }
}
