using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Enemy[] enemies;
    NavMeshAgent navMeshAgent;
    Rigidbody rb;

    public int hp;
    int index = -1;

    bool isMoving;
    bool isRoting;
    
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

    void Awake()
    {
        instance = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        Move();
    }

    public void Move()
    {
        index++;
        enemies[index].isTarget = true;
        if (index < enemies.Length) isMoving = true;
        else Debug.LogError("Out of Array " + index + " / " + enemies.Length);
    }

    public Transform weapon;
    bool isDrag;

    Vector3 startInput;
    Vector3 startPosition;
    Vector3 startRotation;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            startInput = Input.mousePosition;
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

            weapon.localPosition = Vector3.Lerp(weapon.localPosition, new Vector3(0, startPosition.y + yPositiion, startPosition.z + xPosition), 0.35f);
        }

        // di chuyển về phía enemy, khi đến gần nhau thì dừng lại
        if (isMoving)
        {
            navMeshAgent.isStopped = false;
            Vector3 target = enemies[index].transform.position;
            navMeshAgent.SetDestination(target);
            if (Vector3.Distance(new Vector3(transform.position.x, target.y, transform.position.z), target) < GameController.instance.distanceToKill && !navMeshAgent.isOnOffMeshLink)
            {
                navMeshAgent.isStopped = true;
                isMoving = false;
                isRoting = true;
            }
        }

        //khi đến dừng gần lại, thì quay nếu k đủ góc
        if (isRoting)
        {
            Vector3 target = enemies[index].transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.35f);
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            if (angle < 1)
            {
                isRoting = false;
            }
        }
    }



    public void SubtractHp()
    {
        enemies[index].SubtractHp(1, null);
    }
}
