using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSwitcher : MonoBehaviour
{
    public Animator _animator;


    public void StartAnimation(AnimationTypes type)
    {
        if (_animator == null)
        {
            return;
        }
        _animator.Play(type.ToString());
    }

    public void StartLoopMode()
    {
        if (_animator == null)
        {
            return;
        }
        _animator.SetBool("LoopMode", true);
    }

}
