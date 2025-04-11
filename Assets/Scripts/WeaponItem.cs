using UnityEngine;
using static GameController;

public class WeaponItem : MonoBehaviour
{
    public WeaponType weaponType;
    public GameObject unlock;
    public GameObject frameSelect;
    public GameObject frameNoSelect;
    public GameObject unlockGold;

    public void OnClick()
    {
        if (UIController.instance.shop.isRandom || unlock.activeSelf) return;
        UIController.instance.shop.WeaponSelect(weaponType);
    }
}
