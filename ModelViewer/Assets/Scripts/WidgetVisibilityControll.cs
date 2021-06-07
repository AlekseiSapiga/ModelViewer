using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WidgetVisibilityControll : MonoBehaviour
{
    private ScrollRect _scrollRect = null;
    private Vector3[] _viewPortCorners = null;
    private string _imageId;
    private bool _visible = false;

    public void Init(ScrollRect scrollRect, string imageId)
    {
        if (!scrollRect)
        {
            return;
        }
        _imageId = imageId;
        _scrollRect = scrollRect;
        _viewPortCorners = new Vector3[4];
        _scrollRect.gameObject.GetComponent<RectTransform>().GetWorldCorners(_viewPortCorners);
        _scrollRect.onValueChanged.AddListener(OnScrollerChanged);
        _visible = IsVisible();
    }

    private void OnScrollerChanged(Vector2 position)
    {
        if (Math.Abs(position.y) <= 0.01f || !_scrollRect)
        {
            return;
        }
        var newVisibleValue = IsVisible();
        if (newVisibleValue != _visible)
        {
            Renderer.Instance.SetVisible(_imageId, newVisibleValue);
            Debug.Log(_imageId + " | " + IsVisible());
        }
        _visible = newVisibleValue;        
    }

    private bool IsVisible()
    {
        var currentWidgetCorner = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(currentWidgetCorner);

        var viewPortTop = _viewPortCorners[0].y;
        var viewPortBottom = _viewPortCorners[1].y;

        var widgetTop = currentWidgetCorner[0].y;
        var widgetBottom = currentWidgetCorner[1].y;
        if (widgetBottom < viewPortTop || widgetTop > viewPortBottom)
        {
            return false;
        }
        return true;        
    }
}

