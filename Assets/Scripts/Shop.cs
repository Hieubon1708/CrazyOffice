using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject panel;

    public GameObject[] lightsSelect;

    public void LoadData()
    {

    }

    public void Show()
    {
        panel.SetActive(true);
    }
    
    public void Hide()
    {
        panel.SetActive(false);
    }

    public void ResetLightsSelectBox()
    {
        for (int i = 0; i < lightsSelect.Length; i++)
        {
            lightsSelect[i].SetActive(false);
        }
    }
}
