using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressChild : MonoBehaviour
{
    GameObject complete;
    GameObject active;
    TextMeshProUGUI level;

    public void Awake()
    {
        active = transform.GetChild(0).gameObject;
        complete = transform.GetChild(1).gameObject;
        level = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(bool isComplete,bool isActive, int level)
    {
        active.SetActive(isActive);
        complete.SetActive(isComplete);
        this.level.text = level.ToString();
    }
}
