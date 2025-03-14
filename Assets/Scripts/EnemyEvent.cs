using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    Enemy enemy;
    GameObject obj;
    public Transform hand;
    Object scObj;

    public void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void Init()
    {
        int index = Random.Range(0, GameController.instance.preObjectToThrows.Length);
        obj = Instantiate(GameController.instance.preObjectToThrows[index], hand);
        scObj = obj.GetComponent<Object>();
        scObj.targetHead = enemy.head.transform.position;
    }

    public void Throw()
    {
        scObj.Throw();
    }

    public void ExcludePlayerWeapon()
    {
        enemy.ExcludePlayerWeapon(false);
    }

    public void Kill()
    {
        PlayerController.instance.Die();
    }
}
