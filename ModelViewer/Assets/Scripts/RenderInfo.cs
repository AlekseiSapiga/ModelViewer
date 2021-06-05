using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderInfo
{
    public Rect _uvRect { get; private set; }
    public RenderTexture _texture { get; private set; }

    private bool _isValid = false;
    public RenderInfo() { _isValid = false; }

    public RenderInfo(Rect uvRect, RenderTexture texture)
    {
        _uvRect = uvRect;
        _texture = texture;
        _isValid = true;
    }

    public bool IsValid()
    {
        return _isValid;
    }


}
