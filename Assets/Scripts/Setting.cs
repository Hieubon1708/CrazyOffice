using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject panel;

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
}
