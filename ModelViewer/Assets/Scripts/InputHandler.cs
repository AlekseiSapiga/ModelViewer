using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface ITouch
{
    bool IsTouchDown();
    bool IsTouchUp();
    Vector2 GetPosition();
}

class MouseTouch : ITouch
{
    public bool IsTouchDown()
    {
        return Input.GetMouseButtonDown(0);
    }
    public bool IsTouchUp()
    {
        return Input.GetMouseButtonUp(0);
    }
    public Vector2 GetPosition()
    {
        return Input.mousePosition;
    }
}

class MobileTouch : ITouch
{
    public bool IsTouchDown()
    {
        if (Input.touchCount == 0)
        {
            return false;
        }

        return Input.touches[0].phase == TouchPhase.Began;
    }
    public bool IsTouchUp()
    {
        if (Input.touchCount == 0)
        {
            return false;
        }
        return Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled;
    }
    public Vector2 GetPosition()
    {
        if (Input.touchCount == 0)
        {
            return Vector2.zero;
        }
        return Input.touches[0].position;
    }
}