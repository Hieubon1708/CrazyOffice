using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public void Awake()
    {
        SceneManager.LoadScene(1);
    }
}
