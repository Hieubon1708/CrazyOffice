using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    GameObject obj;
    public Transform hand;
    Object scObj;

    public void Init(Vector3 headPos)
    {
        int index = Random.Range(0, GameController.instance.preObjectToThrow.Length);
        obj = Instantiate(GameController.instance.preObjectToThrow[index], hand);
        scObj = obj.GetComponent<Object>();
        scObj.targetHead = headPos;
    }

    public void Throw()
    {
        scObj.Throw();
    }
}
