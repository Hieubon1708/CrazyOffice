using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    Enemy enemy;

    public void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default") && !enemy.isCollision)
        {
            if (collision.relativeVelocity.magnitude < 5f) return;
            enemy.isCollision = true;
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.bodyFalls, 0);

            DOVirtual.DelayedCall(Random.Range(0.5f, 0.75f), delegate
            {
                enemy.isCollision = false;
            });
        }
    }
}
