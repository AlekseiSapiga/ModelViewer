using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class KineticRotation : MonoBehaviour
{
    public float _maxVelocity = 700.0f;
    public float _deceleration = 950.0f;
    public float _filter = 0.5f;

    private float _mousePosition = 0.0f;
    private float _scrollVelocity = 0.0f;
    
    private bool _scrolling = false;
    private bool _stopping = false;
    private ITouch _touchHandler = null;
    public RectTransform _inputRect;

    public void Init(KineticRotationDB settings, RectTransform inputRect)
    {
        _maxVelocity = settings._maxVelocity;
        _deceleration = settings._deceleration;
        _filter = settings._filter;
        _inputRect = inputRect;
    }

    void Start()
    {
        if (Input.touchSupported)
        {
            _touchHandler = new MobileTouch();
        }
        else
        {
            _touchHandler = new MouseTouch();            
        }        
    }

    void Update()
    {
        if (_touchHandler == null)
        {
            return;
        }        
        if (_touchHandler.IsTouchDown())
        {
            _scrollVelocity = 0.0f;
            _mousePosition = _touchHandler.GetPosition().x;
            if (_inputRect != null)
            {
                _scrolling = RectTransformUtility.RectangleContainsScreenPoint(_inputRect, _touchHandler.GetPosition());
            }
            else
            {
                _scrolling = true;
            }            
        }

        if (_scrolling)
        {
            float deltaPosition = _touchHandler.GetPosition().x - _mousePosition;
            
            float velocity = (deltaPosition / Time.deltaTime);
            _mousePosition = Input.mousePosition.x;

            _scrollVelocity = _filter * velocity + (1.0f - _filter) * _scrollVelocity;


            if (Math.Abs(_scrollVelocity) > _maxVelocity)
            {
                var dir = _scrollVelocity / Math.Abs(_scrollVelocity);
                _scrollVelocity = dir * _maxVelocity;
            }
        }

        if (_touchHandler.IsTouchUp() && _scrolling)
        {
            _scrolling = false;
            _stopping = true;
        }

        if (_scrolling || _stopping)
        {
            var curAngles = transform.eulerAngles;
            curAngles.y -= _scrollVelocity * Time.deltaTime;
            if (Math.Abs(_scrollVelocity) > 0.0f && _stopping)
            {
                var dir = _scrollVelocity / Math.Abs(_scrollVelocity);
                _scrollVelocity -= dir * _deceleration  * Time.deltaTime;
            }
            else
            {
                _stopping = false;
                _scrollVelocity = 0.0f;
            }

            transform.eulerAngles = curAngles;
        }
    }
}
