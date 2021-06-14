using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class CharacterLookAt : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;
    public Transform lookObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }
            }
            else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}

