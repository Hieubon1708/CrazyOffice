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
            PlayerPrefs.SetInt("Level", value);
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

    public WeaponType CurrentWeapon
    {
        get
        {
            return (WeaponType)PlayerPrefs.GetInt("CurrentWeapon", (int)WeaponType.a);
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

    public bool IsAtiveSound
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
        }
    }

    public bool IsActiveVibrate
    {
        get
        {
            return PlayerPrefs.GetInt("Vibrate", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Vibrate", value ? 1 : 0);
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
