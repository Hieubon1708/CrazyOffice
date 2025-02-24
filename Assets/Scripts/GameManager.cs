using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt("Level", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Player", value);
        }
    }

    public int Gold
    {
        get
        {
            return PlayerPrefs.GetInt("Gold", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Gold", value);
        }
    }

    public int Cash
    {
        get
        {
            return PlayerPrefs.GetInt("Cash", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Cash", value);
        }
    }

    public WeaponType CurrentWeapon
    {
        get
        {
            return (WeaponType)PlayerPrefs.GetInt("CurrentWeapon", (int)WeaponType.A);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentWeapon", (int)value);
        }
    }

    public List<WeaponType> WeaponsUnlocked
    {
        get
        {
            string txt = PlayerPrefs.GetString("WeaponsUnlocked", string.Empty);
            if (!string.IsNullOrEmpty(txt))
            {
                return JsonConvert.DeserializeObject<List<WeaponType>>(txt);
            }

            return new List<WeaponType>();
        }
        set
        {
            string txt = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString("WeaponsUnlocked", txt);
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            List<WeaponType> weaponsUnlocked = new List<WeaponType>(WeaponsUnlocked);
            weaponsUnlocked.Add(WeaponType.A);
            WeaponsUnlocked = weaponsUnlocked;
            Debug.Log(WeaponsUnlocked.Count);
        }*/
    }
}
