using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerController.instance.countStuff < 5)
        {
            PlayerController.instance.countStuff++;
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.stuff, 0);
        }
    }
}
