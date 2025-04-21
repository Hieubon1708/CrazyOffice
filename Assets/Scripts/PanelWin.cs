using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameController;

public class PanelWin : MonoBehaviour
{
    public CanvasGroup panelWin;

    public CanvasGroup imageNext;
    public CanvasGroup imageGet;

    public CanvasGroup barClaim;

    public TextMeshProUGUI goldReceive;
    public TextMeshProUGUI goldReceiveBoss;
    public TextMeshProUGUI goldReceiveBossButton;

    public RectTransform iconGold;
    public RectTransform startPositionGoldFly;

    public RectTransform[] goldsFly;

    public ObjectReceive[] objectReceives;

    public GameObject noThanks;

    public GameObject like;

    public RectTransform objectReceiveParent;

    public RectTransform arrow;
    public RectTransform buttonClaim;
    public RectTransform claim;

    public GameObject percent;

    int mul;

    bool isClaim;

    [HideInInspector]
    public bool isTweening;

    public void Awake()
    {
        objectReceives = GetComponentsInChildren<ObjectReceive>(true);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        isClaim = false;

        isTweening = true;

        panelWin.alpha = 0f;

        objectReceiveParent.localScale = Vector3.one;

        noThanks.SetActive(false);

        imageNext.gameObject.SetActive(false);
        imageGet.gameObject.SetActive(false);

        barClaim.gameObject.SetActive(false);
        goldReceive.gameObject.SetActive(false);

        imageNext.DOFade(0f, 0f);
        imageGet.DOFade(0f, 0f);

        barClaim.DOFade(0f, 0f);

        int percentReceiveObject = GameManager.instance.PercentReceiveObject;

        bool isFullPercent = percentReceiveObject == 100;

        List<GameController.WeaponType> listUnlocks = GameManager.instance.WeaponsUnlocked;

        List<GameController.WeaponType> allWeapons = ((GameController.WeaponType[])Enum.GetValues(typeof(GameController.WeaponType))).ToList();

        foreach (var w in listUnlocks)
        {
            allWeapons.Remove(w);
        }

        if ((percentReceiveObject == 0 || !allWeapons.Contains(GameManager.instance.CurrentReceiveObject)) && allWeapons.Count > 0)
        {
            int random = (int)allWeapons[UnityEngine.Random.Range(0, allWeapons.Count)];

            GameManager.instance.CurrentReceiveObject = (GameController.WeaponType)random;
        }

        GameController.WeaponType weaponType = GameManager.instance.CurrentReceiveObject;

        percent.SetActive(allWeapons.Count > 0);

        LoadPercent(weaponType, percentReceiveObject, allWeapons.Count == 0);

        panelWin.DOFade(1f, 0.5f).OnComplete(delegate
        {
            if (isFullPercent && allWeapons.Count > 0)
            {
                GameManager.instance.PercentReceiveObject = 0;

                goldReceive.gameObject.SetActive(false);

                Action action = () =>
                {
                    objectReceiveParent.DOScale(1.25f, 0.5f).OnComplete(delegate
                    {
                        imageGet.gameObject.SetActive(true);
                        imageGet.DOFade(1f, 0.25f);

                        DOVirtual.DelayedCall(1.5f, delegate
                        {
                            if (imageGet.gameObject.activeSelf) noThanks.SetActive(true);
                        });
                    }).SetDelay(0.5f);
                };

                if (allWeapons.Count > 0) IncreasePercent(action, weaponType, percentReceiveObject);
                else action.Invoke();
            }
            else if ((GameManager.instance.Level - 1) % 5 == 0)
            {
                goldReceive.gameObject.SetActive(false);

                barClaim.gameObject.SetActive(true);

                LaunchProgress();

                Action action = () =>
                {
                    barClaim.DOFade(1f, 0.25f).SetDelay(0.5f);
                };
                if (allWeapons.Count > 0) IncreasePercent(action, weaponType, percentReceiveObject);
                else action.Invoke();
            }
            else
            {
                Action action = () =>
                {
                    goldReceive.gameObject.SetActive(true);

                    goldReceive.text = "+" + (percentReceiveObject - 25) + " <sprite=0>";

                    DOVirtual.Int(0, UIController.instance.totalEarn, 0.5f, (v) =>
                    {
                        goldReceive.text = "+" + v + " <sprite=0>";
                    }).OnComplete(delegate
                    {
                        GoldFly(startPositionGoldFly.position, null);
                    });
                };

                if (allWeapons.Count > 0) IncreasePercent(action, weaponType, percentReceiveObject);
                else action.Invoke();
            }

            isTweening = false;
        });
    }

    public void Hide()
    {
        isTweening = true;

        panelWin.DOFade(0f, 0.5f).OnComplete(delegate
        {
            gameObject.SetActive(false);

            isTweening = false;
        });
    }

    public void NoThanks()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        objectReceiveParent.localScale = Vector3.one;

        imageGet.gameObject.SetActive(false);

        barClaim.gameObject.SetActive(false);

        noThanks.SetActive(false);

        List<GameController.WeaponType> allWeapons = ((GameController.WeaponType[])Enum.GetValues(typeof(GameController.WeaponType))).ToList();

        int random = (int)allWeapons[UnityEngine.Random.Range(0, allWeapons.Count)];

        GameManager.instance.CurrentReceiveObject = (GameController.WeaponType)random;

        LoadPercent(GameManager.instance.CurrentReceiveObject, 0, allWeapons.Count == 0);

        goldReceive.gameObject.SetActive(true);

        goldReceive.text = "+0 <sprite=0>";

        DOVirtual.Int(0, UIController.instance.totalEarn, 0.5f, (v) =>
        {
            goldReceive.text = "+" + v + " <sprite=0>";
        }).OnComplete(delegate
        {
            GoldFly(startPositionGoldFly.position, null);
        });
    }

    public void GetObject()
    {
        if (ACEPlay.Bridge.BridgeController.instance.IsRewardReady())
        {
            UnityEvent eReward = new UnityEvent();
            eReward.AddListener(() =>
            {
                List<GameController.WeaponType> weaponTypes = GameManager.instance.WeaponsUnlocked;

                weaponTypes.Add(GameManager.instance.CurrentReceiveObject);

                GameManager.instance.WeaponsUnlocked = weaponTypes;
                GameManager.instance.CurrentWeapon = GameManager.instance.CurrentReceiveObject;

                NoThanks();
            });
            ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);
        }
        else
        {
            List<GameController.WeaponType> weaponTypes = GameManager.instance.WeaponsUnlocked;

            weaponTypes.Add(GameManager.instance.CurrentReceiveObject);

            GameManager.instance.WeaponsUnlocked = weaponTypes;
            GameManager.instance.CurrentWeapon = GameManager.instance.CurrentReceiveObject;

            NoThanks();
        }
    }

    void GoldFly(Vector3 startPosition, Action callback)
    {
        int count = 0;

        for (int i = 0; i < goldsFly.Length; i++)
        {
            Vector3 random = UnityEngine.Random.insideUnitSphere;

            random.z = goldsFly[i].position.z;

            int index = i;

            goldsFly[index].gameObject.SetActive(true);
            goldsFly[index].position = startPosition;

            goldsFly[index].DOMove(startPosition + random, 0.35f).OnComplete(delegate
            {
                goldsFly[index].DOMove(iconGold.position, 1f).SetDelay(UnityEngine.Random.Range(0.15f, 0.75f)).SetEase(Ease.InBack).OnComplete(delegate
                {
                    goldsFly[index].gameObject.SetActive(false);

                    iconGold.DOKill();

                    iconGold.DOScale(0.4f, 0.15f).OnComplete(delegate
                    {
                        iconGold.DOScale(0.35f, 0.15f);
                    });

                    count++;

                    if (count == 1)
                    {
                        UIController.instance.ShowGoldIncrease();
                    }

                    if (count == goldsFly.Length - 1)
                    {
                        if (callback != null) callback.Invoke();

                        imageNext.gameObject.SetActive(true);
                        imageNext.DOFade(1f, 0.25f);
                    }

                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.goldRewards, 50);
                });
            });
        }
    }

    void IncreasePercent(Action action, GameController.WeaponType weaponType, int percentReceiveObject)
    {
        for (int i = 0; i < objectReceives.Length; i++)
        {
            if (i == (int)weaponType)
            {
                objectReceives[i].Increase(percentReceiveObject, action);
            }
        }
    }

    void LoadPercent(GameController.WeaponType weaponType, int percentReceiveObject, bool isFull)
    {
        like.SetActive(isFull);

        for (int i = 0; i < objectReceives.Length; i++)
        {
            if (i == (int)weaponType && !isFull)
            {
                objectReceives[i].IsActive(true);
                objectReceives[i].LoadData(percentReceiveObject);
            }
            else
            {
                objectReceives[i].IsActive(false);
            }
        }
    }

    public void LaunchProgress()
    {
        int totalEarn = UIController.instance.totalEarn;

        goldReceiveBoss.text = "GET +" + totalEarn + " <sprite=0>";

        arrow.DOLocalMoveX(380f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnUpdate(delegate
        {
            if (arrow.anchoredPosition.x >= -86 && arrow.anchoredPosition.x <= 86)
            {
                if(mul != 5)
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.progresses, 50);
                }
                mul = 5;
            }
            else if (arrow.anchoredPosition.x >= -202 && arrow.anchoredPosition.x < -86 || arrow.anchoredPosition.x <= 202 && arrow.anchoredPosition.x > 86)
            {
                if (mul != 4)
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.progresses, 50);
                }
                mul = 4;
            }
            else if (arrow.anchoredPosition.x >= -321 && arrow.anchoredPosition.x < -202 || arrow.anchoredPosition.x <= 321 && arrow.anchoredPosition.x > 202)
            {
                if (mul != 3)
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.progresses, 50);
                }
                mul = 3;
            }
            else
            {
                if (mul != 2)
                {
                    AudioController.instance.PlaySoundNVibrate(AudioController.instance.progresses, 50);
                }
                mul = 2;
            }
            
            goldReceiveBossButton.text = "Claim " + "\n+" + (totalEarn * mul) + " <sprite=0>";
        });
    }

    public void Claim(bool isButton)
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        if (isClaim) return;
        if (isButton)
        {
            if (ACEPlay.Bridge.BridgeController.instance.IsRewardReady())
            {
                UnityEvent eReward = new UnityEvent();
                eReward.AddListener(() =>
                {
                    ClaimHandle(isButton);

                });
                ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);
            }
            else
            {
                ClaimHandle(isButton);
            }
         ;
        }
        else
        {
            ClaimHandle(isButton);
        }
    }

    void ClaimHandle(bool isButton)
    {
        isClaim = true;

        UIController.instance.totalEarn *= (mul - 1);
        GameManager.instance.Gold += UIController.instance.totalEarn;

        arrow.DOKill();

        Action action = () =>
        {
            barClaim.gameObject.SetActive(false);
        };

        GoldFly(!isButton ? claim.position : buttonClaim.position, action);
    }
}
