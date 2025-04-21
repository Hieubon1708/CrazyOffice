using DG.Tweening;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Bot
{
    public GameController.HpType hpType;
    public GameController.IdleType idleType;

    [HideInInspector]
    public int hp;
    int startHp;

    public bool isThrowObject;

    public Transform spine;

    public Rigidbody rbHat;

    public Rigidbody rbArmor;

    EnemyEvent enemyEvent;

    [HideInInspector]
    public bool isExcludeLayer;

    bool isPrepareForBattle;

    LayerMask weaponLayer;

    [HideInInspector]
    public bool isCollision;

    public override void Awake()
    {
        base.Awake();

        enemyEvent = GetComponentInChildren<EnemyEvent>();

        weaponLayer = LayerMask.GetMask("Weapon");

        hp = GameController.instance.GetHp(hpType);
        startHp = hp;

        GameController.instance.SetClothes(head, spine, hpType, this);
    }

    public void Start()
    {
        animator.SetInteger("IdleId", GameController.instance.GetIndexIdle(idleType));
        animator.SetTrigger("Idle");
    }

    public void SubtractHp(int hp, Vector2 dir, Rigidbody rb)
    {
        if (this.hp <= 0) return;

        ExcludePlayerWeapon(true);

        DOVirtual.DelayedCall(0.5f, delegate
        {
            ExcludePlayerWeapon(false);
            isTarget = true;
        }).SetUpdate(true);

        if (hp == 1)
        {
            this.hp -= hp;
            if (this.hp == 1)
            {
                animator.SetTrigger("HitDouble");
                FallEquip();
            }
            else if (this.hp == 2)
            {
                animator.SetTrigger("HitDouble");
                FallEquip();
            }
            else
            {
                Die(rb, dir);

                int gold = Random.Range(20, 30);

                UIController.instance.totalEarn += gold;
            }
        }
        else
        {
            if (rbArmor != null) FallArmor();
            if (rbHat != null) FallHat();

            UIController.instance.totalEarn += Random.Range(20, 30);

            Die(rb, dir);
        }
    }

    void Die(Rigidbody rb, Vector2 dir)
    {
        animator.enabled = false;
        navMeshAgent.enabled = false;

        puppetMaster.state = PuppetMaster.State.Dead;
        puppetMaster.mode = PuppetMaster.Mode.Active;

        puppetMaster.muscleSpring = 0;
        puppetMaster.muscleDamper = 50;

        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].excludeLayers = weaponLayer;
            rbs[i].isKinematic = false;
        }

        Vector3 localForce = transform.TransformDirection(new Vector3(-dir.x * 200f, dir.y * 200f, Random.Range(-200f, -250f)));

        rb.AddForce(localForce, ForceMode.Impulse);

        PlayerController.instance.StopMove();

        DOVirtual.DelayedCall(2f, delegate
        {
            PlayerController.instance.ResumeMove();
            PlayerController.instance.Move();
        }).SetUpdate(true);
    }

    void FallEquip()
    {
        if (startHp == 3)
        {
            if (hp == 1)
            {
                FallArmor();
            }
            else if (hp == 2)
            {
                FallHat();
            }
        }
        else
        {
            FallHat();
        }
    }

    public void FallArmor()
    {
        rbArmor.isKinematic = false;

        rbArmor.transform.SetParent(GameController.instance.levelObject.transform);

        rbArmor.angularVelocity = RandomAngularVelocity() * 5;

        Vector3 localForce = transform.TransformDirection(new Vector3(Random.Range(-2f, 2f), Random.Range(7f, 8f), Random.Range(-7f, -8f)));

        rbArmor.AddForce(localForce, ForceMode.Impulse);
    }

    public void FallHat()
    {
        rbHat.isKinematic = false;

        rbHat.transform.SetParent(GameController.instance.levelObject.transform);

        rbHat.angularVelocity = RandomAngularVelocity() * 5;

        Vector3 localForce = transform.TransformDirection(new Vector3(Random.Range(-2f, 2f), Random.Range(7f, 8f), Random.Range(-7f, -8f)));

        rbHat.AddForce(localForce, ForceMode.Impulse);
    }

    public void ExcludePlayerWeapon(bool isExclude)
    {
        isExcludeLayer = isExclude;
    }

    public void AfterHit()
    {
        isTarget = true;
    }

    public void FixedUpdate()
    {
        if (PlayerController.instance != null && navMeshAgent.enabled)
        {
            if (isTarget)
            {
                // check khoảng cách nếu đến 1 khoảng thì chuẩn bị tư thế tiến đấu, hoặc ném vật
                Vector3 target = PlayerController.instance.transform.position;

                float distance = Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z));

                if (distance <= distanceReady)
                {
                    if (!isPrepareForBattle)
                    {
                        // nếu là ném thì dừng di chuyển của player tạm thời
                        if (isThrowObject)
                        {
                            PlayerController.instance.StopMove();

                            if (PlayerController.instance.isLookAt)
                            {
                                enemyEvent.Init();
                                animator.SetTrigger("Throw");

                                isPrepareForBattle = true;
                            }
                        }
                        else
                        {
                            animator.SetTrigger("Walk");
                            isPrepareForBattle = true;
                        }
                    }

                    // đủ khoảng cách thì luôn look at vào th player
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
                    float angle = Quaternion.Angle(transform.rotation, targetRotation);

                    if (distance <= GameController.instance.distanceToKill)
                    {
                        isTarget = false;

                        PlayerController.instance.StopMove();
                        Kill();
                    }
                }
            }
        }
    }

    public void FightAgain()
    {
        isThrowObject = false;
        isPrepareForBattle = false;
    }

    void Kill()
    {
        string aniName = Random.Range(0, 2) == 0 ? "Punch_Left" : "Punch_Right";
        animator.SetTrigger(aniName);
    }

    public void DieByObject(Rigidbody rb)
    {
        SubtractHp(10, Vector2.zero, rb);
    }

    Vector3 RandomAngularVelocity()
    {
        return new Vector3(
           Random.Range(-1f, 1f),
           Random.Range(-1f, 1f),
           Random.Range(-1f, 1f)
       ).normalized;
    }
}
