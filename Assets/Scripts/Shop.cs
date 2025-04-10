using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject panel;

    public GameObject notice;

    public GameObject backgroundShop;

    WeaponItem[] weaponItems;


    private void Awake()
    {
        weaponItems = GetComponentsInChildren<WeaponItem>();
    }

    public void LoadData()
    {
        List<GameController.WeaponType> weaponTypes = GameManager.instance.WeaponsUnlocked;

        if (weaponTypes.Count == 0)
        {
            weaponTypes.Add(GameController.WeaponType.a);

            GameManager.instance.WeaponsUnlocked = weaponTypes;
        }

        foreach (var weaponItem in weaponItems)
        {
            if (weaponTypes.Contains(weaponItem.weaponType)) weaponItem.unlock.SetActive(false);
            else weaponItem.unlock.SetActive(true);
        }

        GameController.WeaponType currentWeapon = GameManager.instance.CurrentWeapon;

        CheckNotice();
    }

    public void WeaponSelect(GameController.WeaponType weaponType)
    {

    }

    public void Show()
    {
        panel.SetActive(true);
        backgroundShop.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        backgroundShop.SetActive(false);
    }

    void CheckNotice()
    {
        int gold = GameManager.instance.Gold;

        notice.SetActive(gold > 1);
    }
}
