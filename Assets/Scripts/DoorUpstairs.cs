using DG.Tweening;
using UnityEngine;

public class DoorUpstairs : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {
            doors[0].DOLocalRotate(new Vector3(doors[0].localEulerAngles.x - 150f, doors[0].localEulerAngles.y, doors[0].localEulerAngles.z), 1f).SetEase(Ease.Linear).SetUpdate(true);

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.ironDoor, 0);

            if (doors.Length == 2)
            {
                doors[1].DOLocalRotate(new Vector3(doors[1].localEulerAngles.x + 150f, doors[1].localEulerAngles.y, doors[1].localEulerAngles.z), 1f).SetEase(Ease.Linear).SetUpdate(true);
            }
        }
    }
}
