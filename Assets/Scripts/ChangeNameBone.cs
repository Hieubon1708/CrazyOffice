using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNameBone : MonoBehaviour
{
    public GameObject old;
    public Transform[] boneOld;
    public void Awake()
    {
        boneOld = old.transform.GetChild(1).GetComponentsInChildren<Transform>();

        for (int i = 0; i < boneOld.Length; i++)
        {
            if (boneOld[i].name.Contains("mixamorig:"))
            {
                boneOld[i].name = boneOld[i].name.Replace("mixamorig:", "");
                Debug.Log(boneOld[i].name);
            }
        }
    }
}
