using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class ViewPortSetter : MonoBehaviour
{
    private Camera _camera;
    
    public Light _light;
    public Vector2Int _layerMaskRange = new Vector2Int(8, 32);
    public int _prevLayerMask = -1;
    public RenderInfo _renderInfo;


    private void SetLayerMask(int layerMask)
    {
        if (_prevLayerMask == layerMask)
        {
            return;
        }
        _prevLayerMask = layerMask;

        gameObject.layer = layerMask;
        var allChildrenGO = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in allChildrenGO)
        {
            child.gameObject.layer = layerMask;
        }

        _camera.cullingMask = 1 << layerMask;

        if (_light)
        {
            _light.cullingMask = 1 << layerMask;
        }
    }

    public void Set(RenderInfo renderInfo)
    {
        if (!renderInfo.IsValid())
        {
            gameObject.SetActive(false);
            return;
        }
        if (!_camera)
        {
            _camera = gameObject.GetComponentInChildren<Camera>();
        }
        gameObject.SetActive(true);

        SetLayerMask(Mathf.Clamp(_layerMaskRange.x + renderInfo._renderIndex, _layerMaskRange.x, _layerMaskRange.y));

        _camera.rect = renderInfo._uvRect;
        _camera.targetTexture = renderInfo._texture;
    }
}
