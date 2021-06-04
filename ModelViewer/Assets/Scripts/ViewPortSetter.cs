using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ViewPortSetter : MonoBehaviour
{    
    public Vector2 offset;
    public Vector2 count;
    public int _layerMask;
    public Camera _camera;
    public RenderTexture _texture;
    
    void Start()
    {                

        if (!_camera || !_texture || count.x < 0.1f || count.y < 0.1f)
        {
            return;
        }

        gameObject.layer = _layerMask;
        var allChildrenGO = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in allChildrenGO)
        {
            child.gameObject.layer = _layerMask;
        }

        _camera.targetTexture = _texture;
        _camera.cullingMask = 1 << _layerMask;

        var oneOverCount = new Vector2(1.0f, 1.0f) / count;                
        _camera.rect = new Rect(oneOverCount.x * offset.x, oneOverCount.x * offset.y, oneOverCount.x, oneOverCount.x);
    }

    
}
