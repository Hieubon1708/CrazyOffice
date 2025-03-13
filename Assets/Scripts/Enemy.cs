using DG.Tweening;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int hp;
    public GameController.IdleType idleType;

    public bool isThrowObject;

    public Rigidbody rbFirstProtective;
    public Rigidbody rbSecondProtective;

    Animator animator;
    public Rigidbody[] rbs;
    NavMeshAgent navMeshAgent;
    ThrowObject enemyHand;
    PuppetMaster puppetMaster;

    [HideInInspector]
    public bool isTarget;

    public float distanceReady = 10f;
    public float playerAngularSpeed;
    public float playerStartSpeed;

    bool isPrepareForBattle;
    float timeToKill;

    LayerMask weaponLayer;

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rbs = GetComponentsInChildren<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        puppetMaster = GetComponentInChildren<PuppetMaster>();
        if (isThrowObject) enemyHand = GetComponentInChildren<ThrowObject>();

        weaponLayer = LayerMask.GetMask("Weapon");
    }

    public void Start()
    {
        animator.SetInteger("IdleId", GameController.instance.GetIndexIdle(idleType));
        animator.SetTrigger("Idle");
    }

    public void SubtractHp(int hp, Vector2 dir, Rigidbody rb)
    {
        if (this.hp <= 0) return;
        //Debug.Log(rb.name);
        this.hp -= hp;
        if (this.hp == 2)
        {
            rbFirstProtective.isKinematic = false;
        }
        else if (this.hp == 1)
        {
            rbSecondProtective.isKinematic = false;
        }
        else
        {
            animator.enabled = false;
            navMeshAgent.enabled = false;

            puppetMaster.state = PuppetMaster.State.Dead;
            puppetMaster.mode = PuppetMaster.Mode.Active;

            puppetMaster.muscleSpring = 0;

            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].excludeLayers = weaponLayer;
                rbs[i].isKinematic = false;
            }

            Vector3 localForce = transform.TransformDirection(new Vector3(-dir.x * 200f, dir.y * 200f, Random.Range(-200f, -250f)));

            rb.AddForce(localForce, ForceMode.Impulse);

            DOVirtual.DelayedCall(2.5f, delegate
            {
                PlayerController.instance.ResumeMove();
                PlayerController.instance.Move(true);
            });
        }
    }

    public void Update()
    {
        Debug.Log(PlayerController.instance.isMoving);
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerController.instance.ResumeMove();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerController.instance.StopMove();
            PlayerController.instance.ResetPath();
        }
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
                            PlayerController.instance.ResetPath();

                            if (PlayerController.instance.isLookAt)
                            {
                                PlayerController.instance.StopMove();

                                /*enemyHand.Init(rbs[9].transform.position);
                                animator.SetTrigger("Throw");*/

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

                    // nếu đủ tầm thì chạy vào gần player, 2 con tiến vào nhau cùng lúc
                    /*if (angle < 1 && distance > GameController.instance.distanceToKill && PlayerController.instance.navMeshAgent.updatePosition)
                    {
                        navMeshAgent.SetDestination(PlayerController.instance.transform.position);
                    }

                    // đủ tầm thì stop r tấn công
                    if (distance <= GameController.instance.distanceToKill)
                    {
                        navMeshAgent.isStopped = true;
                        timeToKill += Time.fixedDeltaTime;
                        if (timeToKill >= 1)
                        {
                            isTarget = false;
                            Kill();
                        }
                    }*/
                }
            }
        }
    }

    void Kill()
    {
        /* Debug.Log("Kill");
         string aniName = Random.Range(0, 2) == 0 ? "Punch_Left" : "Punch_Right";
         animator.SetTrigger(aniName);*/
    }

    public void Damage()
    {
        Debug.Log("Damage");
    }

    public void DieByObject(Rigidbody rb)
    {
        SubtractHp(1, Vector2.zero, rb);
    }
}
