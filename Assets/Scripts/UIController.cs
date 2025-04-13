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

    PanelWin panelWin;
    PanelLose panelLose;
    public GameObject layerCover;

    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textLevel;

    public UIHandTutorial hand;
    public Image fade;

    public Camera camUI;

    public GameObject buttonShop;
    public GameObject buttonNoAds;
    public GameObject buttonSetting;

    [HideInInspector]
    public int totalEarn = 28;

    public Animation fxBlood;

    void Awake()
    {
        instance = this;

        panelWin = GetComponentInChildren<PanelWin>(true);
        panelLose = GetComponentInChildren<PanelLose>(true);
    }

    public void Start()
    {
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

        shop.LoadData();

        shop.CheckNotice();
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
        panelWin.Show();
    }

    void HidePanelWin()
    {
        panelWin.Hide();
    }

    void ShowPanelLose()
    {
        panelLose.Show();
    }

    void HidePanelLose()
    {
        panelLose.Hide();
    }

    public void NextLevel()
    {
        if (panelWin.isTweening) return;

        HidePanelWin();
        GameController.instance.LoadLevel(GameController.instance.GetLevel());
    }

    public void Replay()
    {
        if (panelLose.isTweening) return;

        HidePanelLose();
        GameController.instance.LoadLevel(GameController.instance.GetLevel());
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
