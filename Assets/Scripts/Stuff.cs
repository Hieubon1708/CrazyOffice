using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    bool isAddForce;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerController.instance.countStuff < 5 && !isAddForce) 
        {
            isAddForce = true;

            Vector3 dir = transform.position - collision.gameObject.transform.position;

            GetComponent<Rigidbody>().AddForce(dir * 15f, ForceMode.Impulse);

            PlayerController.instance.countStuff++;
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.stuff, 0);
        }
    }
}
