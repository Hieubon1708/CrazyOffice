using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public List<Collider> collidersInContact = new List<Collider>();

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
}
