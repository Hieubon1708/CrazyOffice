using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform door;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            door.DOLocalMoveX(0.0261f, 1f).SetEase(Ease.Linear);
        }
    }
}
