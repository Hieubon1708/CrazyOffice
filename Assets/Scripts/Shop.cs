using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject panel;

    public GameObject notice;

    WeaponItem[] weaponItems;

    [HideInInspector]
    public bool isRandom;

    int weaponCost = 1500;
    public TextMeshProUGUI textWeaponCost;

    public Image[] buttonBuy;
    public TextMeshProUGUI textBuy;

    public GameObject[] buttons;

    private void Awake()
    {
        weaponItems = GetComponentsInChildren<WeaponItem>(true);
        textWeaponCost.text = "OPEN \n" + weaponCost;
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
            if (weaponTypes.Contains(weaponItem.weaponType))
            {
                weaponItem.unlock.SetActive(false);
                weaponItem.frameNoSelect.SetActive(true);
            }
            else
            {
                weaponItem.unlock.SetActive(true);
                weaponItem.frameNoSelect.SetActive(false);
            }
        }

        GameController.WeaponType currentWeapon = GameManager.instance.CurrentWeapon;

        WeaponSelect(currentWeapon, false);

        CheckButtonBuy();
    }

    public void WeaponSelect(GameController.WeaponType weaponType, bool isButton)
    {
        if (isButton) AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        foreach (var weaponItem in weaponItems)
        {
            if (weaponItem.weaponType == weaponType)
            {
                weaponItem.frameSelect.SetActive(true);

                GameManager.instance.CurrentWeapon = weaponItem.weaponType;
            }
            else weaponItem.frameSelect.SetActive(false);
        }
    }

    void CheckButtonBuy()
    {
        if (GetWeaponUnlock().Count == 0)
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);

            return;
        }
        else
        {
            int gold = GameManager.instance.Gold;

            bool isOk = gold >= weaponCost;

            Color color = Color.white;

            if (!isOk) color = new Vector4(0.5f, 0.5f, 0.5f, 1f);

            foreach (var e in buttonBuy)
            {
                e.color = color;
            }

            textBuy.color = color;
        }
    }

    public void Show()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        LoadData();
        panel.SetActive(true);
    }

    public void Hide()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        panel.SetActive(false);

        PlayerController.instance.InitWeapon();
    }
    public void CheckNotice()
    {
        int gold = GameManager.instance.Gold;

        notice.SetActive(gold >= weaponCost && GetWeaponUnlock().Count > 0);
    }

    public List<int> GetWeaponUnlock()
    {
        List<int> ints = new List<int>();

        for (int i = 0; i < weaponItems.Length; i++)
        {
            if (weaponItems[i].unlock.activeSelf) ints.Add(i);
        }

        return ints;
    }

    public IEnumerator RandomWeapon()
    {
        int count = Random.Range(25, 35);

        List<int> ints = GetWeaponUnlock();

        if (ints.Count == 0) yield break;

        isRandom = true;

        bool isOne = ints.Count == 1;

        int temp = -1;

        for (int i = 0; i < count; i++)
        {
            AudioController.instance.PlaySoundNVibrate(AudioController.instance.roll, 50);

            int index = ints[Random.Range(0, ints.Count)];

            for (int j = 0; j < ints.Count; j++)
            {
                if ((GameController.WeaponType)ints[j] == (GameController.WeaponType)index) weaponItems[ints[j]].unlockGold.SetActive(true);
                else weaponItems[ints[j]].unlockGold.SetActive(false);
            }

            if (temp != -1)
            {
                ints.Add(temp);
                weaponItems[temp].unlockGold.SetActive(false);
            }

            temp = index;

            ints.Remove(index);

            yield return new WaitForSeconds(0.25f - i * 0.005f);

            if (i == count - 1 || isOne)
            {
                yield return new WaitForSeconds(0.5f);

                AudioController.instance.PlaySoundNVibrate(AudioController.instance.unlockItem, 50);

                weaponItems[index].unlock.SetActive(false);
                weaponItems[index].frameNoSelect.SetActive(true);

                List<GameController.WeaponType> weaponTypes = GameManager.instance.WeaponsUnlocked;

                weaponTypes.Add(weaponItems[index].weaponType);

                GameManager.instance.WeaponsUnlocked = weaponTypes;

                UIController.instance.shop.WeaponSelect(weaponItems[index].weaponType, false);

                isRandom = false;

                if (isOne)
                {
                    yield break;
                }
            }
        }
    }

    public void GoldReward()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        if (ACEPlay.Bridge.BridgeController.instance.IsRewardReady())
        {
            UnityEvent eReward = new UnityEvent();
            eReward.AddListener(() =>
            {
                GameManager.instance.Gold += 500;

                UIController.instance.UpdateGold();

                CheckButtonBuy();

                AudioController.instance.PlaySoundNVibrate(AudioController.instance.goldReward, 50);
            });
            ACEPlay.Bridge.BridgeController.instance.ShowRewarded("placement", eReward, null);
        }
        else
        {
            GameManager.instance.Gold += 500;

            UIController.instance.UpdateGold();

            CheckButtonBuy();

            AudioController.instance.PlaySoundNVibrate(AudioController.instance.goldReward, 50);
        }
    }

    public void Roll()
    {
        if (GameManager.instance.Gold < weaponCost || isRandom) return;

        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        GameManager.instance.Gold -= weaponCost;

        UIController.instance.UpdateGold();

        CheckButtonBuy();

        StartCoroutine(RandomWeapon());
    }
}
