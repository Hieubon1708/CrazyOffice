using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Enemy[] enemies;
    NavMeshAgent navMeshAgent;

    public int hp;
    int index;


    void Awake()
    {
        instance = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
    }

    public void Move()
    {
        enemies[index].isReady = true;
        if (index < enemies.Length) navMeshAgent.SetDestination(enemies[index].transform.position);
        else Debug.LogError("!");
    }

    public Camera cam;
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
            weapon.localRotation = Quaternion.Lerp(weapon.localRotation, newLocalRotation, 0.1f);

            //Position
            float xPosition = (currentInput.x - startInput.x) * 0.0005f;
            float yPositiion = (startInput.y - currentInput.y) * 0.0005f;

            weapon.localPosition = Vector3.Lerp(weapon.localPosition, new Vector3(0, startPosition.y + yPositiion, startPosition.z + xPosition) , 0.1f);

        }
    }
}
