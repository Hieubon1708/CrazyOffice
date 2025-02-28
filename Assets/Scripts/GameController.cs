using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float distanceToKill;
    public GameObject levelObject;

    public GameObject[] preObjectToThrow;

    void Awake()
    {
        instance = this;
    }

    public enum WeaponType
    {
        A, B, C, None
    }

    public enum ModeType
    {
        Normal, Boss, None
    }

    public enum IdleType
    {
        ListenToThePhone, ShoulderRub, BendDownToDoSomething, RaiseRightHand, None
    }

    public void Start()
    {
        LoadLevel(GameManager.instance.Level);
    }

    public void LoadLevel(int level)
    {
        UIController.instance.LoadData();
        
        if(levelObject != null) Destroy(levelObject);

        levelObject = Instantiate(Resources.Load<GameObject>(level.ToString()));
    }

    public int GetIndexIdle(IdleType idleType)
    {
        switch (idleType)
        {
            case IdleType.ListenToThePhone: return 1;
            case IdleType.ShoulderRub: return 8;
            case IdleType.BendDownToDoSomething: return 5;
            case IdleType.RaiseRightHand: return 7;
        }
        return 0;
    }
}
