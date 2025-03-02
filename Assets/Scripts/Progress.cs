using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    Image[] dots;

    private void Awake()
    {
        dots = GetComponentsInChildren<Image>();
    }

    public void LoadData()
    {
        int index = GameManager.instance.Level;

        while (index > 5)
        {
            index -= 5;
        }

        for (int i = 0; i < dots.Length; i++)
        {
            if(i < index)
            {
                dots[i].color = Color.red;
            }
            else
            {
                dots[i].color = Color.white;
            }
        }
    }
}
