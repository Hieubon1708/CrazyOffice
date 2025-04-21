using UnityEngine;

public class FootEvent : MonoBehaviour
{
    public void AfterKickRight()
    {
        Boss4 boss4 = PlayerController.instance.CurrentBoss as Boss4;

        transform.parent.gameObject.SetActive(false);

        boss4.AfterKickRight();
    }
}
