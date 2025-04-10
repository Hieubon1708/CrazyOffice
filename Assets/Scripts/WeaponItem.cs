using UnityEngine;
using static GameController;

public class WeaponItem : MonoBehaviour
{
    public WeaponType weaponType;
    public GameObject unlock;
    public GameObject frameSelect;

    public void OnClick()
    {
        UIController.instance.ChooseWeapon(weaponType);
    }
}
