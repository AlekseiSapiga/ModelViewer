using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ViewPortSetter : MonoBehaviour
{
    private Camera _camera;
    private static  int _layerMask = 8;

    //public Vector2 offset;
    //public Vector2 count;    
    //public RenderTexture _texture;
    public RenderInfo _renderInfo;

    private void StartInt()
    {
        _camera = gameObject.GetComponentInChildren<Camera>();
        if (!_camera)// || _renderInfo == null || !_renderInfo.IsValid()
        {
            return;
        }

        gameObject.layer = _layerMask;
        var allChildrenGO = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in allChildrenGO)
        {
            child.gameObject.layer = _layerMask;
        }

        //_camera.targetTexture = _renderInfo._texture;
        _camera.cullingMask = 1 << _layerMask;

        _layerMask++;
        if (_layerMask >= 32)
        {
            _layerMask = 8;
        }
      // var oneOverCount = new Vector2(1.0f, 1.0f) / _renderInfo._count;
      //_camera.rect = _renderInfo._uvRect;
      //new Rect(oneOverCount.x * _renderInfo._offset.x, oneOverCount.x * _renderInfo._offset.y, oneOverCount.x, oneOverCount.x);
    }

    public void Set(RenderInfo renderInfo)
    {
        if (!renderInfo.IsValid())
        {
            return;
        }
        StartInt();
        _camera.rect = renderInfo._uvRect;
        _camera.targetTexture = renderInfo._texture;
    }
}
