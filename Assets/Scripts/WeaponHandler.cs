using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public List<Collider> collidersInContact = new List<Collider>();

    Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Die()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        rb.angularVelocity = RandomAngularVelocity() * 10;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (!collidersInContact.Contains(collision.collider))
        {
            collidersInContact.Add(collision.collider);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (collidersInContact.Contains(collision.collider))
        {
            collidersInContact.Remove(collision.collider);
        }
    }

    void Update()
    {
        //Debug.Log("Số collider đang va chạm: " + collidersInContact.Count);
        PlayerController.instance.isCollision = collidersInContact.Count > 0;
    }

    Vector3 RandomAngularVelocity()
    {
        return new Vector3(
           Random.Range(-1f, 1f),
           Random.Range(-1f, 1f),
           Random.Range(-1f, 1f)
       ).normalized;
    }
}
