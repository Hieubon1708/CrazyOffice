using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    Animator animator;
    float distance = 10f;
    public bool isReady;
    bool isBoxing;
    Rigidbody[] rbs;

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        //animator.SetInteger("IdleId", Random.Range(0, 10));
        rbs = GetComponentsInChildren<Rigidbody>();
        IsKinematic(true);
    }

    void IsKinematic(bool isKinematic)
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            if(rbs[i].name.Contains("Target")) continue;
            rbs[i].isKinematic = isKinematic;
        }
    }

    void UseGravity(bool isUse)
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].useGravity = isUse;
        }
    }

    public void SubtractHp(int hp, Transform killer)
    {
        if (this.hp <= 0) return;
        this.hp -= hp;
    }

    public void Update()
    {
        if (PlayerController.instance != null)
        {
            if (isReady)
            {
                float distance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
                if (distance <= this.distance)
                {
                    if (!isBoxing)
                    {
                        animator.SetTrigger("Walk");
                        isBoxing = true;
                    }
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(PlayerController.instance.transform.position), 0.1f);
                }
            }
        }
    }
}
