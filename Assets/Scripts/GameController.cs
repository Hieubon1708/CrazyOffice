using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

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
}
