using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource music;
    public AudioSource src;
    public AudioSource srcLoop;

    public AudioClip clickButton;
    public AudioClip unlockItem;
    public AudioClip roll;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip increasePercent;
    public AudioClip completePercent;
    public AudioClip[] goldRewards;
    public AudioClip[] hits;
    public AudioClip[] hitsFish;
    public AudioClip[] hitsChick;
    public AudioClip[] hitsBall;
    public AudioClip[] hitsMine;
    public AudioClip[] swings;
    public AudioClip[] walks;
    public AudioClip[] bgs;
    public AudioClip[] slaps;
    public AudioClip progresses;
    public AudioClip goldReward;
    public AudioClip hitHeadBoss;
    public AudioClip openDoor;
    public AudioClip[] bodyFalls;
    public AudioClip downstairs;
    public AudioClip stuff;
    public AudioClip flushToilet;
    public AudioClip electricity;
    public AudioClip behead;
    public AudioClip waterSplash;
    public AudioClip balloon;
    public AudioClip ironSlideDoor;
    public AudioClip ironDoor;

    Coroutine coroutineWalk;

    public void Awake()
    {
        instance = this;
    }

    public void PlayMusic()
    {
        music.clip = bgs[UnityEngine.Random.Range(0, bgs.Length)];
        music.Play();
    }
    
    public void ResumeMusic()
    {
        music.mute = false;
    }

    public void StopMusic()
    {
        music.mute = true;
    }

    public void PlayWalk()
    {
        coroutineWalk = StartCoroutine(Walk());
    }

    IEnumerator Walk()
    {
        int index = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.35f);

            src.PlayOneShot(walks[index]);

            index++;

            if (index == walks.Length) index = 0;
        }
    }

    public IEnumerator PlaySoundNVibrate(AudioClip[] audioClips, int strength, int count, float timeDelay)
    {
        while (count > 0)
        {
            PlaySoundNVibrate(audioClips[UnityEngine.Random.Range(0, audioClips.Length)], strength);

            count--;

            yield return new WaitForSeconds(timeDelay);
        }
    }

    public void StopWalk()
    {
        StopCoroutine(coroutineWalk);
    }

    public void PlaySoundNVibrate(AudioClip audioClip, int strength)
    {
        if (audioClip != null && GameManager.instance.IsAtiveSound)
        {
            src.PlayOneShot(audioClip);
        }
        if (GameManager.instance.IsActiveVibrate)
        {
            if (strength != 0) Duc.Vibration.Vibrate(strength);
        }
    }
    
    public void PlaySoundNVibrateLoop(AudioClip audioClip, int strength)
    {
        if (audioClip != null && GameManager.instance.IsAtiveSound)
        {
            srcLoop.clip = audioClip;
            srcLoop.Play();
        }
        if (GameManager.instance.IsActiveVibrate)
        {
            if (strength != 0) Duc.Vibration.Vibrate(strength);
        }
    } 
    
    public void PlayVibrate(int strength)
    {
        if (GameManager.instance.IsActiveVibrate)
        {
            Duc.Vibration.Vibrate(strength);
        }
    }

    public void StopSrcLoop()
    {
        srcLoop.Stop();
    }

    public void PlaySoundNVibrate(AudioClip[] audioClips, int strength)
    {
        if (GameManager.instance.IsAtiveSound)
        {
            src.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length)]);
        }
        if (GameManager.instance.IsActiveVibrate)
        {
            if (strength != 0) Duc.Vibration.Vibrate(strength);
        }
    }

    public AudioClip GetHit(GameController.WeaponType weaponType)
    {
        if(weaponType == GameController.WeaponType.f)
        {
            return hitsFish[UnityEngine.Random.Range(0, hits.Length)];
        }else
        if(weaponType == GameController.WeaponType.g)
        {
            return hitsChick[UnityEngine.Random.Range(0, hits.Length)];
        }

        return hits[UnityEngine.Random.Range(0, hits.Length)];
    }
}
