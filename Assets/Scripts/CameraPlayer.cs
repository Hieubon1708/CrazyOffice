using DG.Tweening;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider col;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<BoxCollider>();
    }

    public void Die()
    {
        rb.isKinematic = false;
        col.enabled = true;

        Vector3 dir = PlayerController.instance.Dir;

        rb.velocity = dir * 3;

        transform.DOShakeRotation(0.5f);
    }
}
