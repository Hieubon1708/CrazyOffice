using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downstairs : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.downstairs, 0);
        }
    }
}
