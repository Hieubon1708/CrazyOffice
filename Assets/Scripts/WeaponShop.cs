using UnityEngine;
using static GameController;

public class WeaponShop : MonoBehaviour
{
    public WeaponType weaponType;
    public GameObject lightSelect;

    public void OnClick()
    {
        UIController.instance.ChooseWeapon(weaponType);
        lightSelect.SetActive(true);
    }
}
