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
    public TextMeshProUGUI textLevel;

    [HideInInspector]
    public UIHandTutorial uIHandTutorial;

    public UIHandTutorial hand;
    public Image fade;

    public Camera camUI;

    public GameObject buttonShop;
    public GameObject buttonNoAds;
    public GameObject buttonSetting;

    int totalEarn = 28;

    void Awake()
    {
        instance = this;
        uIHandTutorial = GetComponentInChildren<UIHandTutorial>();
    }

    public void Start()
    {
        shop.LoadData();
        setting.LoadData();

        UpdateGold();
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

    public void UpdateGold()
    {
        textGold.text = GameManager.instance.Gold.ToString();
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
        GameManager.instance.Level++;
        GameManager.instance.PercentReceiveObject += 25;
        GameManager.instance.Gold += totalEarn;

        DOVirtual.DelayedCall(1f, delegate
        {
            ShowPanelWin();
        });
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

    public void ShowGoldIncrease()
    {
        int totalGold = GameManager.instance.Gold;

        DOVirtual.Int(totalGold - totalEarn, totalGold, 0.5f, (v) =>
        {
            textGold.text = v.ToString();
        });
    }
}
