using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlickAnimation : MonoBehaviour
{
    public float BlickPosition;
    private Material _material;
    private Animation _animation;

    void Start()
    {
        _material = GetComponent<Image>().material;
        _animation = GetComponent<Animation>();
    }

    void Update()
    {
        if (_material != null)
        {
            _material.SetFloat("_offsetX", BlickPosition);
        }
    }

    public void AnimateBlick(bool value)
    {
        if (_animation == null)
        {
            return;
        }
        if (value)
        {
            _animation.Play();
        }
        else
        {
            _animation.Stop();
        }        
    }
}
