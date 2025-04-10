using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [HideInInspector]
    public UIHandTutorial uIHandTutorial;

    public UIHandTutorial hand;
    public Image fade;

    public Camera camUI;

    public GameObject buttonShop;
    public GameObject buttonNoAds;
    public GameObject buttonSetting;

    void Awake()
    {
        instance = this;
        uIHandTutorial = GetComponentInChildren<UIHandTutorial>();
    }

    public void Start()
    {
        shop.LoadData();
        setting.LoadData();
    }

    void FirstUI(bool isActive)
    {
        progress.gameObject.SetActive(isActive);
        buttonShop.SetActive(isActive);
        buttonNoAds.SetActive(isActive);
        buttonSetting.SetActive(isActive);
        fade.gameObject.SetActive(isActive);
        hand.gameObject.SetActive(isActive);
    }

    public void LoadData()
    {
        textLevel.text = "Level " + GameManager.instance.Level;
        progress.LoadData();

        hand.Show();

        FirstUI(true);
    }

    public void Play()
    {
        FirstUI(false);

        PlayerController.instance.Move();
    }

    public void Win()
    {
        //GameManager.instance.Level++;
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
