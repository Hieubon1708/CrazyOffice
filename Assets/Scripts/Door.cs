using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isOpen;

    public Transform[] doors;

    public void Start()
    {
        foreach (var door in doors)
        {
            door.gameObject.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true;

            doors[0].DOLocalRotate(new Vector3(doors[0].localEulerAngles.x, doors[0].localEulerAngles.y - 150f, doors[0].localEulerAngles.z), 1f).SetEase(Ease.Linear).SetUpdate(true);

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.openDoor, 0);

            if (doors.Length == 2)
            {
                doors[1].DOLocalRotate(new Vector3(doors[1].localEulerAngles.x, doors[1].localEulerAngles.y + 150f, doors[1].localEulerAngles.z), 1f).SetEase(Ease.Linear).SetUpdate(true);
            }
        }
    }
}
