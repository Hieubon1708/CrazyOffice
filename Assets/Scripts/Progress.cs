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

        int n = GetMul(level);

        for (int i = 0; i < progressChildrens.Length; i++)
        {
            progressChildrens[i].SetValue(n * 5 + i < level - 1, n * 5 + i < level, n * 5 + i + 1);
        }
    }

    int GetMul(int level)
    {
        int result = 1;

        while (true)
        {
            if (result * 5 >= level)
            {
                return result - 1;
            }
            result++;
        }
    }
}
