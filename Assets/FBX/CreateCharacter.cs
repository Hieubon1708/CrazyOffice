using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class CreateCharacter : MonoBehaviour
{
    public GameObject character;

    public Transform puppetMaster;

    public PhysicMaterial physicMaterial;

    Transform[] child;

    public List<Transform> childList = new List<Transform>();

    void Start()
    {
        PuppetMaster puppetMaster = character.GetComponentInChildren<PuppetMaster>();
        Enemy enemy = character.GetComponent<Enemy>();

        child = character.GetComponentsInChildren<Transform>();

        enemy.head = GetObj("mixamorig:HeadTop_End");
        enemy.spine = GetObj("mixamorig:Spine1");

        Transform pN = GetObj("PuppetMaster");

        Transform[] bones = pN.GetComponentsInChildren<Transform>();
        Transform[] bonesOld = this.puppetMaster.GetComponentsInChildren<Transform>();

        puppetMaster.targetRoot = character.transform.GetChild(2);

        for (int i = 1; i < bones.Length; i++)
        {
            ConfigurableJoint jointO = bonesOld[i].GetComponent<ConfigurableJoint>();
            CapsuleCollider colO = bonesOld[i].GetComponent<CapsuleCollider>();
            BoxCollider boxO = bonesOld[i].GetComponent<BoxCollider>();
            Rigidbody rbO = bonesOld[i].GetComponent<Rigidbody>();

            bool isContainCol = boxO != null || colO != null;

            bool isBox = boxO != null;

            ConfigurableJoint jointN = bones[i].GetComponent<ConfigurableJoint>();
            if (jointN == null) jointN = bones[i].AddComponent<ConfigurableJoint>();

            CapsuleCollider colN = bones[i].GetComponent<CapsuleCollider>();
            BoxCollider boxN = bones[i].GetComponent<BoxCollider>();

            if (boxO != null && boxN == null)
            {
                boxN = bones[i].AddComponent<BoxCollider>();
            }
            
            if (colO != null && colN == null)
            {
                colN = bones[i].AddComponent<CapsuleCollider>();
            }

            Rigidbody rbN = bones[i].GetComponent<Rigidbody>();
            if (rbN == null) rbN = bones[i].AddComponent<Rigidbody>();

            if (isContainCol) EditorUtility.CopySerialized(isBox ? boxO : colO, isBox ? boxN : colN);
        }
    }

    Transform GetObj(string name)
    {
        foreach (Transform t in child)
        {
            if (t.name == name) return t;
        }

        return null;
    }
}
