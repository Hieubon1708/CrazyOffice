using Ara;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public GameController.WeaponType weaponType;

    [HideInInspector]
    public List<Collider> collidersInContact = new List<Collider>();

    Vector3 normal;

    Rigidbody rb;

    bool isThrow;

    [HideInInspector]
    public AraTrail araTrail;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        araTrail = GetComponentInChildren<AraTrail>();

        araTrail.emit = false;
    }

    public void Die()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        rb.angularVelocity = RandomAngularVelocity() * 10;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy") || rb.useGravity) return;
        if (isThrow)
        {
            if (!rb.useGravity)
            {
                PlayerController.instance.CurrentBoss.HitFirst();

                AudioController.instance.PlaySoundNVibrate(AudioController.instance.hitHeadBoss, 50);
            }
            rb.useGravity = true;
        }
        else
        {
            if (!collidersInContact.Contains(collision.collider))
            {
                normal = collision.contacts[0].point;
                collidersInContact.Add(collision.collider);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy") || isThrow) return;
        if (collidersInContact.Contains(collision.collider))
        {
            collidersInContact.Remove(collision.collider);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy") || isThrow) return;
        normal = collision.contacts[0].point;
    }

    public void ThrowStraight(Vector3 dir)
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.swings, 0);

        isThrow = true;
        transform.SetParent(GameController.instance.levelObject.transform);
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = false;
        rb.velocity = dir.normalized * 10;
        rb.angularVelocity = -transform.right * 20;
    }

    void Update()
    {
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

    public void HitFx()
    {
        GameController.instance.HitFx(normal);
    }
}
