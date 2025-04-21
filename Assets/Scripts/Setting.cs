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
        if(time != 0) AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        if (settingOption.type == TypeSetting.Sound)
        {
            GameManager.instance.IsAtiveSound = isActive;

            if (isActive) AudioController.instance.ResumeMusic();
            else AudioController.instance.StopMusic();
        }
        if (settingOption.type == TypeSetting.Vibrate) GameManager.instance.IsActiveVibrate = isActive;
        settingOption.SwitchStateHandle(isActive, time);
    }

    public enum TypeSetting
    {
        None, Sound, Vibrate
    }

    public void Show()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        ACEPlay.Bridge.BridgeController.instance.ShowBannerCollapsible();

        panel.SetActive(true);
    }

    public void Hide()
    {
        AudioController.instance.PlaySoundNVibrate(AudioController.instance.clickButton, 50);

        ACEPlay.Bridge.BridgeController.instance.HideBannerCollapsible();

        panel.SetActive(false);
    }
}
