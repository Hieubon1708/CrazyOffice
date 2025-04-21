using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Bot : MonoBehaviour
{
    [HideInInspector]
    public bool isTarget;

    public float distanceReady = 10f;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public PuppetMaster puppetMaster;

    [HideInInspector]
    public Animator animator;

    public float playerAngularSpeed;
    public float playerStartSpeed;

    protected Rigidbody[] rbs;

    public Transform head;

    public virtual Vector3 TargetRotation
    {
        get
        {
            return rbs[0].position;
        }
    }
    
    public virtual Vector3 TargetPosition
    {
        get
        {
            return transform.position;
        }
    }

    public virtual void Awake()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        puppetMaster = GetComponentInChildren<PuppetMaster>();

        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
}
