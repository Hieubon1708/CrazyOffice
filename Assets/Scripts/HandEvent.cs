using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEvent : MonoBehaviour
{
    public void Slap()
    {
        PlayerController.instance.SlapBoss();
    }

    public void Completely()
    {
        PlayerController.instance.CompletelyAttack();
    }
}
