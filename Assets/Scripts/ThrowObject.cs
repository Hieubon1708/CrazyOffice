using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    GameObject obj;
    Rigidbody rb;
    public Transform hand;

    public void Init()
    {
        int index = Random.Range(0, GameController.instance.preObjectToThrow.Length);
        obj = Instantiate(GameController.instance.preObjectToThrow[index], hand);
        obj.transform.localPosition = Vector3.zero;
        rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Throw()
    {
        rb.isKinematic = false;

        obj.transform.SetParent(GameController.instance.levelObject.transform);

        Vector3 dir = PlayerController.instance.transform.position - hand.position;

        rb.velocity = new Vector3(dir.x, dir.y + 7, dir.z);

        Vector3 randomSpinAxis = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
        rb.angularVelocity = randomSpinAxis * 10;
    }
}
