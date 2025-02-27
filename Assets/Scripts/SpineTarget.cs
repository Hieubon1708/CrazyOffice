using UnityEngine;

public class SpineTarget : MonoBehaviour
{
    public Transform spine;
    bool isCollision;
    Rigidbody rb;

    public void Awake()
    {
         rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        isCollision = true;
        rb.velocity = Vector3.zero;
        Debug.Log("On");
    }

    public void OnCollisionExit(Collision collision)
    {
        isCollision = false;
        rb.velocity = Vector3.zero;
        Debug.LogWarning("Off");
    }

    public void Update()
    {
        if (!isCollision)
        {
            Debug.LogError("X");
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0.85f, 0), 0.1f);
            rb.isKinematic = false;
        }
        //Debug.LogError(Vector3.Distance(transform.position, spine.position));
        if (Vector3.Distance(transform.position, spine.position) > 2)
        {
            rb.isKinematic = true;
        }
    }
}
