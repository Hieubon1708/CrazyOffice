using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float distanceToKill;

    [HideInInspector]
    public GameObject levelObject;

    public GameObject[] preObjectToThrows;
    public GameObject[] prePlayerWeapons;
    public ParticleSystem fxWeapon;
    public ParticleSystem fxHitFly;

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
        a, b, c, d, e, f, g, h, i
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
        ACEPlay.Bridge.BridgeController.instance.ShowBanner();

        LoadLevel(GetLevel());
    }

    public int GetLevel()
    {
        int level = GameManager.instance.Level;
        if (level > 20)
        {
            GameManager.instance.Level = 1;
            GameManager.instance.ReplayLevel++;

            return 1;
        }

        return level;
    }

    public void LoadLevel(int level)
    {
        if (levelObject != null) Destroy(levelObject);

        levelObject = Instantiate(Resources.Load<GameObject>(level.ToString()), transform);
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

    public void HitFx(Vector3 position)
    {
        Vector3 dir = PlayerController.instance.cameraPlayer.transform.position - position;

        fxWeapon.transform.position = position + dir.normalized * 0.5f;

        fxWeapon.Play();
    }
}
