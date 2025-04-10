using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    ProgressChild[] progressChildrens;

    private void Awake()
    {
        progressChildrens = GetComponentsInChildren<ProgressChild>();
    }

    public void LoadData()
    {
        int level = GameManager.instance.Level;

        int n = level / 5;

        for (int i = 0; i < progressChildrens.Length; i++)
        {
            progressChildrens[i].SetValue(n * 5 + i < level, n * 5 + i + 1);
        }
    }
}
