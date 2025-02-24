using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameController;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Setting setting;
    public Shop shop;
    public Office office;

    public GameObject panelWin;
    public GameObject panelLose;
    public GameObject layerCover;

    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textCash;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadData();
    }

    void LoadData()
    {
        shop.LoadData();
        setting.LoadData();

        textGold.text = GameManager.instance.Gold.ToString();
        textCash.text = GameManager.instance.Cash.ToString();
    }

    void ShowPanelWin()
    {
        panelWin.SetActive(true);
    }

    void HidePanelWin()
    {
        panelWin.SetActive(false);
    }

    void ShowPanelLose()
    {
        panelLose.SetActive(true);
    }

    void HidePanelLose()
    {
        panelLose.SetActive(false);
    }

    public void AddWeapon(WeaponType weaponType)
    {
        List<WeaponType> weaponsUnlocked = new List<WeaponType>(GameManager.instance.WeaponsUnlocked);
        weaponsUnlocked.Add(weaponType);
        GameManager.instance.WeaponsUnlocked = weaponsUnlocked;
    }

    public void ChooseWeapon(WeaponType weaponType)
    {
        GameManager.instance.CurrentWeapon = weaponType;
        shop.ResetLightsSelectBox();
        //
    }
}
