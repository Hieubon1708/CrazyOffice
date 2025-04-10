using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressChild : MonoBehaviour
{
    GameObject active;
    TextMeshProUGUI level;

    public void Awake()
    {
        active = transform.GetChild(0).gameObject;
        level = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(bool isActive, int level)
    {
        active.SetActive(isActive);
        this.level.text = level.ToString();
    }
}
