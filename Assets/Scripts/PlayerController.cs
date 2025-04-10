using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public const float finalSpeed = 6;

    public Bot[] enemies;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    WeaponHandler weaponHandler;
    [HideInInspector]
    public Hand hand;

    [HideInInspector]
    public CameraPlayer cameraPlayer;

    public int hp;

    [HideInInspector]
    public int index = -1;

    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public bool isRoting;

    [HideInInspector]
    public bool isLookAt;

    public Transform weaponContainer;

    public float Speed
    {
        get
        {
            return navMeshAgent.speed;
        }
        set
        {
            navMeshAgent.speed = value;
        }
    }

    public float AngularSpeed
    {
        get
        {
            return navMeshAgent.angularSpeed;
        }
        set
        {
            navMeshAgent.angularSpeed = value;
        }
    }

    public bool IsUpdatePosition
    {
        get
        {
            return navMeshAgent.updatePosition;
        }
        set
        {
            navMeshAgent.updatePosition = value;
        }
    }

    public bool IsStop
    {
        get
        {
            return navMeshAgent.isStopped;
        }
        set
        {
            navMeshAgent.isStopped = value;
        }
    }

    public Vector3 Destination
    {
        set
        {
            navMeshAgent.SetDestination(value);
        }
    }

    public Vector3 Dir
    {
        get
        {
            return transform.position - enemies[index].transform.position;
        }
    }

    public Boss CurrentBoss
    {
        get
        {
            return enemies[index] as Boss;
        }
    }

    public Enemy CurrentEnemy
    {
        get
        {
            return enemies[index] as Enemy;
        }
    }

    void Awake()
    {
        instance = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
        cameraPlayer = GetComponentInChildren<CameraPlayer>();
        hand = GetComponentInChildren<Hand>();

        Speed = finalSpeed;

        InitWeapon();
    }

    public void Move()
    {
        if (index + 1 == enemies.Length)
        {
            UIController.instance.Win();
            return;
        }

        index++;

        ResetParam();

        enemies[index].isTarget = true;

        AngularSpeed = enemies[index].playerAngularSpeed;
        Speed = enemies[index].playerStartSpeed;
    }

    public void ResetParam()
    {
        isRoting = false;
        isMoving = true;
        totalSpeedTime = 0;
    }

    [HideInInspector]
    public Transform weapon;
    bool isDrag;

    Vector3 startInput;
    Vector3 endInput;
    Vector3 startPosition;
    Vector3 startRotation;

    [HideInInspector]
    public bool isCollision;

    [HideInInspector]
    public float totalSpeedTime;

    bool isCantTouch;
    bool isAttack;

    [HideInInspector]
    public bool isSoloBoss;

    Vector2 dir;

    [HideInInspector]
    public float tRotate = 0.35f;

    public void Update()
    {
        if (!navMeshAgent.enabled || index == -1) return;

        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            isDrag = true;
            startInput = Input.mousePosition;
            endInput = Input.mousePosition;
            startRotation = weapon.localEulerAngles;
            startPosition = weapon.localPosition;

            if (isSoloBoss)
            {
                if (CurrentBoss is Boss4)
                {
                    Boss4 boss4 = (Boss4)CurrentBoss;

                    boss4.HeadDip();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSoloBoss && !isAttack && isDrag)
            {
                isAttack = true;

                if (CurrentBoss is Boss1)
                {
                    UIController.instance.uIHandTutorial.Hide();

                    Vector3 head = CurrentBoss.headStatic;

                    bool isRight = false;

                    if (Vector2.Distance(Input.mousePosition, startInput) < 5f)
                    {
                        startInput = Camera.main.WorldToScreenPoint(head);

                        dir = (startInput - Input.mousePosition).normalized;

                        isRight = Input.mousePosition.x > Screen.width / 2;
                    }
                    else
                    {
                        dir = (Input.mousePosition - startInput).normalized;

                        isRight = startInput.x > Screen.width / 2;
                    }

                    Vector3 pos = transform.position + transform.forward * 0.5f;

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    hand.Slap(new Vector3(pos.x, head.y, pos.z), head, angle + 90, isRight);
                }
                else if(CurrentBoss is Boss2)
                {
                    Boss2 boss2 = (Boss2)CurrentBoss;

                    boss2.EletricShock();
                }
                else if(CurrentBoss is Boss3)
                {
                    Boss3 boss3 = (Boss3)CurrentBoss;

                    boss3.Cut();
                }
                else if(CurrentBoss is Boss4)
                {
                    Boss4 boss4 = (Boss4)CurrentBoss;

                    boss4.HeadDipExit();
                }
            }

            isDrag = false;
        }
        if (isDrag && !isCantTouch)
        {
            Vector3 currentInput = Input.mousePosition;

            //Rotation
            float xRotation = (startInput.x - currentInput.x) * 0.185f;
            float yRotation = (currentInput.y - startInput.y) * 0.3f;

            float clampX = Mathf.Clamp(yRotation + startRotation.x, 0, 85);
            float clampY = Mathf.Clamp(xRotation + startRotation.y, 15, 165);

            Quaternion newLocalRotation = Quaternion.Euler(clampX, clampY, weapon.localEulerAngles.z);
            weapon.localRotation = Quaternion.Lerp(weapon.localRotation, newLocalRotation, 0.35f);

            //Position
            float xPosition = (currentInput.x - startInput.x) * 0.0005f;
            float yPositiion = (startInput.y - currentInput.y) * 0.0005f;

            float xClamp = Mathf.Clamp(startPosition.z + xPosition, -0.5f, 0.5f);
            float yClamp = Mathf.Clamp(startPosition.y + yPositiion, -0.5f, 0f);

            weapon.localPosition = Vector3.Lerp(weapon.localPosition, new Vector3(0, yClamp, xClamp), 0.35f);

            if (isCollision)
            {
                float distance = Vector2.Distance(currentInput, endInput);
                if (distance > 40f && weaponHandler.collidersInContact.Count > 0)
                {
                    isRoting = true;
                    weaponHandler.HitFx();
                    CurrentEnemy.SubtractHp(1, (currentInput - endInput).normalized, weaponHandler.collidersInContact[0].attachedRigidbody);
                }
            }

            endInput = Input.mousePosition;
        }

        Vector3 targetPosition = enemies[index].TargetPosition;

        Vector3 targetRotation = enemies[index].TargetRotation;
        Quaternion targetQuaternion = Quaternion.LookRotation(new Vector3(targetRotation.x, transform.position.y, targetRotation.z) - transform.position);

        // di chuyển về phía enemy, khi đến gần nhau thì dừng lại

        if (isMoving)
        {
            if (Speed < finalSpeed)
            {
                totalSpeedTime += Time.deltaTime;
                Speed = Mathf.Clamp(Speed + totalSpeedTime, 0, finalSpeed);
            }

            Destination = targetPosition;
        }
        else
        {
            isLookAt = (Quaternion.Angle(transform.rotation, targetQuaternion) < 5);

            if (isLookAt)
            {
                isRoting = true;
            }
        }

        //face to face
        if (isRoting)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, tRotate);
        }
    }

    public void ResetSpeed()
    {
        Speed = finalSpeed;
    }

    public void StopMove()
    {
        isMoving = false;
        IsUpdatePosition = false;
    }

    public void ResumeMove()
    {
        ResetSpeed();

        IsUpdatePosition = true;
        isMoving = true;

        navMeshAgent.nextPosition = transform.position;
        Destination = enemies[index].TargetPosition;

    }

    public void FightAgain()
    {
        PlayerController.instance.CurrentEnemy.FightAgain();
    }

    public void InitWeapon()
    {
        if (weaponHandler != null)
        {
            Destroy(weaponHandler.gameObject);
        }

        GameObject weapon = Instantiate(GameController.instance.prePlayerWeapons[(int)GameManager.instance.CurrentWeapon], weaponContainer);
        this.weapon = weapon.transform;

        weapon.transform.localRotation = Quaternion.Euler(35f, 90f, 0f);

        weaponHandler = weapon.GetComponent<WeaponHandler>();
    }

    public void Die()
    {
        if (!navMeshAgent.enabled) return;

        navMeshAgent.enabled = false;
        cameraPlayer.Die();
        weaponHandler.Die();

        DOVirtual.DelayedCall(3.5f, delegate
        {
            UIController.instance.Lose();
        }).SetUpdate(true);
    }

    public IEnumerator SeeBoss()
    {
        PlayerController.instance.tRotate = 0.025f;

        isCantTouch = true;

        StopMove();

        yield return new WaitUntil(() => isLookAt);

        weapon.transform.DOLocalRotateQuaternion(Quaternion.Euler(90f, 90f, 0f), 0.5f).OnComplete(delegate
        {
            weaponHandler.ThrowStraight(CurrentBoss.neck.position - weapon.position);
        }).SetUpdate(true);
    }

    public void SlapBoss()
    {
        if (CurrentBoss is Boss1) (CurrentBoss as Boss1).Hit(dir);
    }

    public void CompletelyAttack()
    {
        isAttack = false;
    }

    public void Strangle()
    {
        hand.Strangle();
    }
}
