using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform[] doors;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            doors[0].DOLocalRotate(new Vector3(doors[0].localEulerAngles.x, doors[0].localEulerAngles.y + 150f, doors[0].localEulerAngles.z), 1f).SetEase(Ease.Linear);
            if (doors.Length == 2)
            {
                doors[1].DOLocalRotate(new Vector3(doors[1].localEulerAngles.x, doors[1].localEulerAngles.y - 150f, doors[1].localEulerAngles.z), 1f).SetEase(Ease.Linear);
            }
        }
    }
}
