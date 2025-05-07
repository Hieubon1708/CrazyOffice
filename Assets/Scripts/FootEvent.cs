using UnityEngine;

public class FootEvent : MonoBehaviour
{
    int count = 0;

    public void AfterKickRight()
    {
        Boss4 boss4 = PlayerController.instance.CurrentBoss as Boss4;

        transform.parent.gameObject.SetActive(false);
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.GetHit(GameController.WeaponType.a), 0);

        if (count == 0)
        {
            boss4.AfterKickRight();
            count++;
        }
        else
        {
            boss4.EndKick();
            count = 0;
        }
    }
}
