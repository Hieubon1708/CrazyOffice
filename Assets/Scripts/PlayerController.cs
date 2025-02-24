using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }
}
