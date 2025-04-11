using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject panel;

    SettingOption[] settingOptions;

    private void Awake()
    {
        settingOptions = GetComponentsInChildren<SettingOption>(true);
    }

    public void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        ChangeSettingOption(settingOptions[0], GameManager.instance.IsActiveVibrate, 0);
        ChangeSettingOption(settingOptions[1], GameManager.instance.IsAtiveSound, 0);
    }

    public void ChangeSettingOption(SettingOption settingOption, bool isActive, float time)
    {
        if (settingOption.type == TypeSetting.Sound) GameManager.instance.IsAtiveSound = isActive;
        if (settingOption.type == TypeSetting.Vibrate) GameManager.instance.IsActiveVibrate = isActive;
        settingOption.SwitchStateHandle(isActive, time);
    }

    public enum TypeSetting
    {
        None, Sound, Vibrate
    }

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
