using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float distanceToKill;

    [HideInInspector]
    public GameObject levelObject;

    public GameObject[] preObjectToThrows;
    public GameObject[] prePlayerWeapons;

    public GameObject preHat1;
    public GameObject preHat2;
    public GameObject preHat3;
    public GameObject preHat4;
    public GameObject preArmor3;
    public GameObject preArmor4;

    void Awake()
    {
        instance = this;
    }

    public enum WeaponType
    {
        BaseballBat, ChibiMoon, Dubinka, Kendo, PoopShit, None
    }

    public enum IdleType
    {
        ListenToThePhone, ShoulderRub, BendDownToDoSomething, RaiseRightHand, None
    }

    public enum HpType
    {
        HP1, HP2T1, HP2T2, HP3T1, HP3T2
    }

    public void Start()
    {
        LoadLevel(GameManager.instance.Level);
    }

    public void LoadLevel(int level)
    {
        UIController.instance.LoadData();

        if (levelObject != null) Destroy(levelObject);

        levelObject = Instantiate(Resources.Load<GameObject>(level.ToString()));
    }

    public int GetHp(HpType hpType)
    {
        switch (hpType)
        {
            case HpType.HP2T1: return 2;
            case HpType.HP2T2: return 2;
            case HpType.HP3T1: return 3;
            case HpType.HP3T2: return 3;
        }
        return 1;
    }

    public int GetIndexIdle(IdleType idleType)
    {
        /*switch (idleType)
        {
            case IdleType.ListenToThePhone: return 1;
            case IdleType.ShoulderRub: return 8;
            case IdleType.BendDownToDoSomething: return 5;
            case IdleType.RaiseRightHand: return 7;
        }*/
        return 0;
    }

    public void SetClothes(Transform head, Transform spine, HpType hpType, Enemy enemy)
    {
        switch (hpType)
        {
            case HpType.HP2T1:
                {
                    enemy.rbHat = Instantiate(preHat1, head).GetComponent<Rigidbody>();
                    return;
                }
            case HpType.HP2T2:
                {
                    enemy.rbHat = Instantiate(preHat2, head).GetComponent<Rigidbody>();
                    return;
                }
            case HpType.HP3T1:
                {
                    enemy.rbHat = Instantiate(preHat3, head).GetComponent<Rigidbody>();
                    enemy.rbArmor = Instantiate(preArmor3, spine).GetComponent<Rigidbody>();
                    return;
                }
            case HpType.HP3T2:
                {
                    enemy.rbHat = Instantiate(preHat4, head).GetComponent<Rigidbody>();
                    enemy.rbArmor = Instantiate(preArmor4, spine).GetComponent<Rigidbody>();
                    return;
                }
        }
    }
}
