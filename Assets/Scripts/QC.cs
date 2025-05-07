using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QC : MonoBehaviour
{
    public GameObject[] canvases;
    bool isActive;

    public static QC instance;
    private bool isAllowKeyCode;

    public void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1920, 1080, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.G))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1920, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.V))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1080, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.N))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1350, 1080, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.B))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1350, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1920, 1080, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.G))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1920, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.V))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1080, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.N))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1350, 1080, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.B))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1350, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].SetActive(isActive);
            }
            isActive = !isActive;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameManager.instance.Level = Mathf.Clamp(GameManager.instance.Level - 1, 1, GameManager.instance.Level);
            GameController.instance.LoadLevel(GameManager.instance.Level);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.instance.Level++;
            GameController.instance.LoadLevel(GameManager.instance.Level);
        }
    }
}
