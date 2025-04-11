using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class PanelWin : MonoBehaviour
{
    public GameObject buttonNext;

    public CanvasGroup imageNext;
    public CanvasGroup imageGet;

    public TextMeshProUGUI goldReceive;

    public RectTransform iconGold;
    public RectTransform startPositionGoldFly;

    public RectTransform[] goldsFly;

    public ObjectReceive[] objectReceives;

    public GameObject buttonGetFullPercent;
    public GameObject noThanks;

    public GameObject like;

    public RectTransform objectReceiveParent;

    public void Awake()
    {
        objectReceives = GetComponentsInChildren<ObjectReceive>(true);
    }

    public void OnEnable()
    {
        objectReceiveParent.localScale = Vector3.one;

        buttonGetFullPercent.SetActive(false);
        noThanks.SetActive(false);
        buttonNext.SetActive(false);

        goldReceive.gameObject.SetActive(true);

        imageNext.DOFade(0f, 0f);
        imageGet.DOFade(0f, 0f);

        int percentReceiveObject = GameManager.instance.PercentReceiveObject;

        bool isFullPercent = percentReceiveObject == 100;

        List<GameController.WeaponType> listUnlocks = GameManager.instance.WeaponsUnlocked;

        List<GameController.WeaponType> allWeapons = ((GameController.WeaponType[])Enum.GetValues(typeof(GameController.WeaponType))).ToList();

        foreach (var w in listUnlocks)
        {
            allWeapons.Remove(w);
        }

        if (percentReceiveObject == 0 || !allWeapons.Contains(GameManager.instance.CurrentReceiveObject))
        {
            int random = (int)allWeapons[UnityEngine.Random.Range(0, allWeapons.Count)];

            GameManager.instance.CurrentReceiveObject = (GameController.WeaponType)random;
        }

        GameController.WeaponType weaponType = GameManager.instance.CurrentReceiveObject;

        allWeapons.Clear();

        LoadPercent(weaponType, percentReceiveObject, allWeapons.Count == 0);

        DOVirtual.DelayedCall(0.5f, delegate
        {
            if (isFullPercent)
            {
                GameManager.instance.PercentReceiveObject = 0;

                goldReceive.gameObject.SetActive(false);

                Action action = () =>
                {
                    objectReceiveParent.DOScale(1.25f, 0.5f).OnComplete(delegate
                    {
                        buttonGetFullPercent.SetActive(true);
                        imageGet.DOFade(1f, 0.25f);

                        DOVirtual.DelayedCall(1.5f, delegate
                        {
                            noThanks.SetActive(true);
                        });
                    }).SetDelay(0.5f);
                };

                IncreasePercent(action, weaponType, percentReceiveObject);
            }
            else
            {
                Action action = () =>
                {
                    DOVirtual.Int(0, 35, 0.5f, (v) =>
                    {
                        goldReceive.text = "+" + v + " <sprite=0>";
                    }).OnComplete(delegate
                    {
                        GoldFly();
                    });
                };

                IncreasePercent(action, weaponType, percentReceiveObject);
            }
        });
    }

    public void NoThanks()
    {
        objectReceiveParent.localScale = Vector3.one;

        buttonGetFullPercent.SetActive(false);
        noThanks.SetActive(false);

        List<GameController.WeaponType> allWeapons = ((GameController.WeaponType[])Enum.GetValues(typeof(GameController.WeaponType))).ToList();

        int random = (int)allWeapons[UnityEngine.Random.Range(0, allWeapons.Count)];

        GameManager.instance.CurrentReceiveObject = (GameController.WeaponType)random;

        LoadPercent(GameManager.instance.CurrentReceiveObject, 25, allWeapons.Count == 0);

        goldReceive.gameObject.SetActive(true);

        DOVirtual.Int(0, 35, 0.5f, (v) =>
        {
            goldReceive.text = "+" + v + " <sprite=0>";
        }).OnComplete(delegate
        {
            GoldFly();
        });
    }

    public void GetObject()
    {
        List<GameController.WeaponType> weaponTypes = GameManager.instance.WeaponsUnlocked;

        weaponTypes.Add(GameManager.instance.CurrentReceiveObject);

        GameManager.instance.WeaponsUnlocked = weaponTypes;

        NoThanks();
    }

    void GoldFly()
    {
        int count = 0;

        for (int i = 0; i < goldsFly.Length; i++)
        {
            Vector3 random = UnityEngine.Random.insideUnitSphere;

            random.z = goldsFly[i].position.z;

            int index = i;

            goldsFly[index].gameObject.SetActive(true);

            goldsFly[index].DOMove(startPositionGoldFly.transform.position + random, 0.35f).OnComplete(delegate
            {
                goldsFly[index].DOMove(iconGold.position, 1f).SetDelay(UnityEngine.Random.Range(0.15f, 0.5f)).SetEase(Ease.InBack).OnComplete(delegate
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
                        buttonNext.SetActive(true);
                        imageNext.DOFade(1f, 0.25f);
                    }
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
                objectReceives[i].gameObject.SetActive(true);
                objectReceives[i].LoadData(percentReceiveObject);
            }
            else
            {
                objectReceives[i].gameObject.SetActive(false);
            }
        }
    }
}
