using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform door;
    bool isOpened;

    public void Awake()
    {
        if(!door.gameObject.activeSelf) door.gameObject.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            isOpened = true;
            door.DOLocalMoveX(0.0261f, 0.5f).SetEase(Ease.Linear).SetUpdate(true);
        }
    }
}
