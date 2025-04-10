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

    Transform[] childAll;
    Transform[] child1;
    Transform[] child2;

    public List<Transform> childList = new List<Transform>();

    void Start()
    {
        PuppetMaster puppetMasterN = character.GetComponentInChildren<PuppetMaster>();
        Enemy enemy = character.GetComponent<Enemy>();

        childAll = character.GetComponentsInChildren<Transform>();

        Transform puperMasterTf = GetObjOfAll("PuppetMaster");

        child1 = puperMasterTf.GetComponentsInChildren<Transform>();
        child2 = character.transform.GetChild(2).GetComponentsInChildren<Transform>();

        character.transform.GetChild(2).GetComponent<EnemyEvent>().hand = GetObjOfAll("mixamorig:RightHandIndex2");

        enemy.head = GetObjOfBone2("mixamorig:HeadTop_End");
        enemy.spine = GetObjOfBone2("mixamorig:Spine1");

        Transform[] bonesOld = this.puppetMaster.GetComponentsInChildren<Transform>();

        puppetMasterN.targetRoot = character.transform.GetChild(2);

        for (int i = 1; i < child1.Length; i++)
        {
            ConfigurableJoint jointO = bonesOld[i].GetComponent<ConfigurableJoint>();
            CapsuleCollider colO = bonesOld[i].GetComponent<CapsuleCollider>();
            BoxCollider boxO = bonesOld[i].GetComponent<BoxCollider>();
            Rigidbody rbO = bonesOld[i].GetComponent<Rigidbody>();

            bool isContainCol = boxO != null || colO != null;

            bool isBox = boxO != null;

            ConfigurableJoint jointN = child1[i].GetComponent<ConfigurableJoint>();
            if (jointN == null) jointN = child1[i].AddComponent<ConfigurableJoint>();

            CapsuleCollider colN = child1[i].GetComponent<CapsuleCollider>();
            BoxCollider boxN = child1[i].GetComponent<BoxCollider>();
/*
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

            EditorUtility.CopySerialized(rbO, rbN);
            EditorUtility.CopySerialized(jointO, jointN);*/

            /*if (i >= 2)
            {
                Rigidbody rbParent = bones[i].parent.GetComponent<Rigidbody>();

                jointN.connectedBody = rbParent;
            }*/
        }
       
        PuppetMaster puppetMasterO = this.puppetMaster.GetComponent<PuppetMaster>();

        for (int i = 0; i < puppetMasterO.muscles.Length; i++)
        {
            string nameMuscles = puppetMasterO.muscles[i].name;

            ConfigurableJoint targetJoint = GetObjOfBone1(nameMuscles).GetComponent<ConfigurableJoint>();
            Transform targetTf = GetObjOfBone2(nameMuscles);

            puppetMasterN.muscles[i].joint = targetJoint;
            puppetMasterN.muscles[i].target = targetTf;
        }
    }

    Transform GetObjOfBone1(string name)
    {
        foreach (Transform t in child1)
        {
            if (t.name == name) return t;
        }

        return null;
    }
    
    Transform GetObjOfAll(string name)
    {
        foreach (Transform t in childAll)
        {
            if (t.name == name) return t;
        }

        return null;
    }
    
    Transform GetObjOfBone2(string name)
    {
        foreach (Transform t in child2)
        {
            if (t.name == name) return t;
        }

        return null;
    }
}
