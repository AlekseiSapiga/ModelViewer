using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderInfo
{
    public Rect _uvRect { get; private set; }
    public RenderTexture _texture { get; private set; }
    public int _renderIndex { get; private set; }
    private bool _isValid = false;
    public RenderInfo() { _isValid = false; }

    public RenderInfo(Rect uvRect, RenderTexture texture, int renderIndex)
    {
        _uvRect = uvRect;
        _texture = texture;
        _renderIndex = renderIndex;
        _isValid = true;
    }

    public bool IsValid()
    {
        return _isValid;
    }


}
