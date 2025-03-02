using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.impulse.magnitude > 35)
        {
            Debug.Log(collision.impulse.magnitude);
            //PlayerController.instance.SubtractHp();
        }
    }
}
