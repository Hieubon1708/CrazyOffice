﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public const float finalSpeed = 6;

    public Enemy[] enemies;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    WeaponHandler weaponHandler;
    CameraPlayer cameraPlayer;

    public int hp;

    [HideInInspector]
    public int index = -1;

    [HideInInspector]
    public bool isMoving;
    bool isRoting;

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

    void Awake()
    {
        instance = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
        cameraPlayer = GetComponentInChildren<CameraPlayer>();

        Speed = finalSpeed;

        InitWeapon();
    }

    public void Start()
    {
        Move();
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

    void ResetParam()
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

    float totalSpeedTime;

    public void Update()
    {
        if (!navMeshAgent.enabled) return;
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            startInput = Input.mousePosition;
            endInput = Input.mousePosition;
            startRotation = weapon.localEulerAngles;
            startPosition = weapon.localPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
        if (isDrag)
        {
            Vector3 currentInput = Input.mousePosition;

            //Rotation
            float xRotation = (startInput.x - currentInput.x) * 0.185f;
            float yRotation = (currentInput.y - startInput.y) * 0.3f;

            float clampX = Mathf.Clamp(yRotation + startRotation.x, 0, 75);
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
                    enemies[index].SubtractHp(1, (currentInput - endInput).normalized, weaponHandler.collidersInContact[0].attachedRigidbody);
                }
            }

            endInput = Input.mousePosition;
        }

        Vector3 targetPosition = enemies[index].transform.position;

        // di chuyển về phía enemy, khi đến gần nhau thì dừng lại
        if (isMoving)
        {
            if (Speed < finalSpeed)
            {
                totalSpeedTime += Time.deltaTime;
                Speed = Mathf.Clamp(Speed + totalSpeedTime, 0, finalSpeed);
                //Debug.Log(Speed);
            }

            IsStop = false;
            Destination = targetPosition;
            if (Vector3.Distance(new Vector3(transform.position.x, targetPosition.y, transform.position.z), targetPosition) < GameController.instance.distanceToKill && !navMeshAgent.isOnOffMeshLink)
            {
                IsStop = true;
                isMoving = false;
                isRoting = true;
            }
        }

        Vector3 targetHip = enemies[index].HipPos;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetHip.x, transform.position.y, targetHip.z) - transform.position);
        isLookAt = (Quaternion.Angle(transform.rotation, targetRotation) < 5);

        //khi đến dừng gần lại, thì quay nếu k đủ góc
        if (isRoting)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.35f);
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
        Destination = enemies[index].transform.position;
    }

    public void FightAgain()
    {
        enemies[index].FightAgain();
    }

    public void InitWeapon()
    {
        if(weaponHandler != null)
        {
            Destroy(weaponHandler.gameObject);
        }

        GameObject weapon = Instantiate(GameController.instance.prePlayerWeapons[(int)GameManager.instance.CurrentWeapon], weaponContainer);
        this.weapon = weapon.transform;
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
        });
    }
}
