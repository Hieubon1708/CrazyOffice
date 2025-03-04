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
    WeaponHandler weaponHandler;

    [HideInInspector]
    public bool isTarget;
    public float distanceReady = 10f;
    bool isPrepareForBattle;
    float timeToKill;

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>(); 
        rbs = GetComponentsInChildren<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        puppetMaster = GetComponentInChildren<PuppetMaster>();
        if (isThrowObject) enemyHand = GetComponentInChildren<ThrowObject>();
    }

    public void Start()
    {
        animator.SetInteger("IdleId", GameController.instance.GetIndexIdle(idleType));
        animator.SetTrigger("Idle");
    }

    public void SubtractHp(int hp, Vector2 dir, Rigidbody rb)
    {
        if (this.hp <= 0) return;
        this.hp -= hp;
        if(this.hp == 2)
        {
            rbFirstProtective.isKinematic = false;
        }
        else if(this.hp == 1)
        {
            rbSecondProtective.isKinematic = false;
        }
        else
        {
            animator.enabled = false;
            navMeshAgent.enabled = false;
            puppetMaster.state = PuppetMaster.State.Dead;

            rb.AddForce(new Vector3(dir.x * 150f, dir.y * 150f, Random.Range(150f, 200f)), ForceMode.Impulse);
        }
    }  

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            enemyHand.Init();
            enemyHand.Throw();
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
                            enemyHand.Init();
                            animator.SetTrigger("Throw");
                            PlayerController.instance.Speed = 0;  
                        }
                        else
                        {
                            animator.SetTrigger("Walk");
                        }
                        isPrepareForBattle = true;
                    }

                    // đủ khoảng cách thì luôn look at vào th player
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
                    float angle = Quaternion.Angle(transform.rotation, targetRotation);

                    // nếu đủ tầm thì chạy vào gần player, 2 con tiến vào nhau cùng lúc
                    if (angle < 1 && distance > GameController.instance.distanceToKill && PlayerController.instance.Speed > 0)
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
                    }
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
}
