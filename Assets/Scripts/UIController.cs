using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameController;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Setting setting;
    public Shop shop;
    public Progress progress;

    public GameObject panelWin;
    public GameObject panelLose;
    public GameObject layerCover;

    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textCash;
    public TextMeshProUGUI textLevel;

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        shop.LoadData();
        setting.LoadData();

        textGold.text = GameManager.instance.Gold.ToString();
        textCash.text = GameManager.instance.Cash.ToString();
    }

    public void LoadData()
    {
        textLevel.text = "Level " + GameManager.instance.Level;
        progress.LoadData();
    }

    public void Win()
    {
        GameManager.instance.Level++;
        ShowPanelWin();
    }
    
    public void Lose()
    {
        ShowPanelLose();
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
    }

    public void NextLevel()
    {
        HidePanelWin();
        GameController.instance.LoadLevel(GameManager.instance.Level);
    }

    public void Replay()
    {
        HidePanelLose();
        GameController.instance.LoadLevel(GameManager.instance.Level);
    }
}
