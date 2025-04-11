using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject panel;

    public GameObject notice;

    WeaponItem[] weaponItems;

    [HideInInspector]
    public bool isRandom;

    private void Awake()
    {
        weaponItems = GetComponentsInChildren<WeaponItem>(true);
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

        WeaponSelect(currentWeapon);

        CheckNotice();
    }

    public void WeaponSelect(GameController.WeaponType weaponType)
    {
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

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);

        PlayerController.instance.InitWeapon();
    }

    void CheckNotice()
    {
        int gold = GameManager.instance.Gold;

        notice.SetActive(gold >= 2000 && GetWeaponUnlock().Count > 0);
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

                weaponItems[index].unlock.SetActive(false);
                weaponItems[index].frameNoSelect.SetActive(true);

                List<GameController.WeaponType> weaponTypes = GameManager.instance.WeaponsUnlocked;

                weaponTypes.Add(weaponItems[index].weaponType);

                GameManager.instance.WeaponsUnlocked = weaponTypes;

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
        GameManager.instance.Gold += 500;

        UIController.instance.UpdateGold();
    }

    public void Roll()
    {
        if (GameManager.instance.Gold < 2000 || isRandom) return;

        GameManager.instance.Gold -= 2000;

        UIController.instance.UpdateGold();

        StartCoroutine(RandomWeapon());
    }
}
