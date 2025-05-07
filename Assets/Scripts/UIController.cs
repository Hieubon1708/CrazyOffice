using ACEPlay.Native;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    public int totalEarn;

    public Animation fxBlood;

    public GameObject buttonAds;

    void Awake()
    {
        instance = this;

        panelWin = GetComponentInChildren<PanelWin>(true);
        panelLose = GetComponentInChildren<PanelLose>(true);
    }

    public void Start()
    {
        UpdateGold();

        buttonAds.SetActive(ACEPlay.Bridge.BridgeController.instance.CanShowAds);
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
        NativeAds.instance.SetPosition(NativeAds.Position.Top);

        totalEarn = 0;

        int replayLevel = GameManager.instance.ReplayLevel;

        textLevel.text = "Level " + (GameManager.instance.Level + replayLevel * 20);
        progress.LoadData();

        hand.Show();

        FirstUI(true);

        shop.LoadData();

        shop.CheckNotice();
    }

    public void Play()
    {
        if (ACEPlay.Bridge.BridgeController.instance.IsInterReady())
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                FirstUI(false);

                PlayerController.instance.Move();
            });
            ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("start_game", e);
        }
        else
        {
            FirstUI(false);

            PlayerController.instance.Move();
        }
    }

    public void Win()
    {
        if (ACEPlay.Bridge.BridgeController.instance.IsInterReady())
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                GameManager.instance.Level++;
                GameManager.instance.PercentReceiveObject += 25;
                GameManager.instance.Gold += totalEarn;

                DOVirtual.DelayedCall(1f, delegate
                {
                    ShowPanelWin();
                });
            });
            ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("win", e);
        }
        else
        {
            GameManager.instance.Level++;
            GameManager.instance.PercentReceiveObject += 25;
            GameManager.instance.Gold += totalEarn;

            DOVirtual.DelayedCall(1f, delegate
            {
                ShowPanelWin();
            });
        }
    }

    public void Lose()
    {
        if (ACEPlay.Bridge.BridgeController.instance.IsInterReady())
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                ShowPanelLose();
            });
            ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("lose", e);
        }
        else
        {
            ShowPanelLose();
        }
    }

    void ShowPanelWin()
    {
        ACEPlay.Bridge.BridgeController.instance.ShowBannerCollapsible();

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.win, 0);

        panelWin.Show();
    }

    void HidePanelWin()
    {
        ACEPlay.Bridge.BridgeController.instance.HideBannerCollapsible();

        panelWin.Hide();
    }

    void ShowPanelLose()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.lose, 0);

        ACEPlay.Bridge.BridgeController.instance.ShowBannerCollapsible();

        panelLose.Show();
    }

    void HidePanelLose()
    {
        ACEPlay.Bridge.BridgeController.instance.HideBannerCollapsible();

        panelLose.Hide();
    }

    public void NextLevel()
    {
        if (panelWin.isTweening) return;

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        if (ACEPlay.Bridge.BridgeController.instance.IsInterReady())
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                HidePanelWin();
                GameController.instance.LoadLevel(GameController.instance.GetLevel());
            });
            ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("next_level", e);
        }
        else
        {
            HidePanelWin();
            GameController.instance.LoadLevel(GameController.instance.GetLevel());
        }
    }

    public void RemoveAds()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            ACEPlay.Bridge.BridgeController.instance.CanShowAds = false;
            buttonAds.SetActive(false);
        });
        ACEPlay.Bridge.BridgeController.instance.PurchaseProduct("remove_ads", e);
    }

    public void Replay()
    {
        if (panelLose.isTweening) return;

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        if (ACEPlay.Bridge.BridgeController.instance.IsInterReady())
        {
            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                HidePanelLose();
                GameController.instance.LoadLevel(GameController.instance.GetLevel());
            });
            ACEPlay.Bridge.BridgeController.instance.ShowInterstitial("replay", e);
        }
        else
        {
            HidePanelLose();
            GameController.instance.LoadLevel(GameController.instance.GetLevel());
        }
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
