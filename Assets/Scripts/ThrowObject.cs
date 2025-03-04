using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    GameObject obj;
    public Transform hand;
    Object scObj;

    public void Init()
    {
        int index = Random.Range(0, GameController.instance.preObjectToThrow.Length);
        obj = Instantiate(GameController.instance.preObjectToThrow[index], hand);
        scObj = obj.GetComponent<Object>();
    }

    public void Throw()
    {
        scObj.Throw();
    }
}
