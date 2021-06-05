using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateImageListener
{
    void UpdateImage();
}


public sealed class Renderer
{
    class RenderInfoInt
    {
        public RenderInfo _info { get; private set; }
        public ViewPortSetter _obj { get; private set; }
        public IUpdateImageListener _listener { get; private set; }
        public bool _isActive { get; set; }
        public RenderInfoInt() { _isActive = false; }
        public RenderInfoInt(RenderInfo info, ViewPortSetter obj, IUpdateImageListener listener)
        {
            _info = info;
            _obj = obj;
            _listener = listener;
            _isActive = false;
        }
    }

    private static Renderer instance = null;
    private Dictionary<string, RenderInfoInt> _goToRender;
    private RenderTexture _texture = null;
    private int _minSize = 256;
    private bool _inited = false;
    private Renderer()  {}

    public static Renderer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Renderer();
            }
            return instance;
        }
    }

    public void Init(RenderTexture texture, int minSize)
    {
        if (_inited)
        {
            return;
        }
        if (!texture || texture.width != texture.height)
        {
            Debug.LogError("Render texture is null or width != height");
            return;
        }

        _texture = texture;
        _minSize = minSize;
        _goToRender = new Dictionary<string, RenderInfoInt>();
        _inited = true;
    }

    public void Register(string id, ViewPortSetter obj, IUpdateImageListener listener)
    {
        if (!_inited || _goToRender.ContainsKey(id))
        {
            Debug.Log("Register return " + id);
            return;
        }

        _goToRender.Add(id, new RenderInfoInt(new RenderInfo(), obj, listener));
        UpdateInfos();
    }

    public void SetVisible(string id, bool value)
    {

    }

    public void Clear()
    {
        _goToRender.Clear();        
    }

    public RenderInfo GetRenderInfo(string id)
    {
        if (!_goToRender.ContainsKey(id))
        {            
            return new RenderInfo();
        }
        return _goToRender[id]._info;
    }

    private int CeilPower2(int x)
    {
        if (x < 2)
        {
            return 1;
        }
        return (int)Math.Pow(2, (int)Math.Log(x - 1, 2) + 1);
    }

    private void UpdateInfos()
    {
        var cnt = _goToRender.Count;
        if (!_inited || cnt == 0)
        {
            return;
        }        

        var cntImagesPerCol = (int)Math.Sqrt(CeilPower2(cnt));
        Debug.Log("cntImagesPerCol " + cntImagesPerCol);
        var calculatedSize = _texture.width / cntImagesPerCol;
        if (calculatedSize < _minSize)
        {
            return;
        }
        var infoEnumerator = _goToRender.GetEnumerator();
        var shift = 1.0f / cntImagesPerCol;
        var newDict = new Dictionary<string, RenderInfoInt>();

        int i = 0;
        int j = 0;
        while (infoEnumerator.MoveNext())
        {
            
            Debug.Log(i + " | " + j);
            
            var rect = new Rect(shift * j, shift * i, shift, shift);

            var renderInfo = new RenderInfo(rect, _texture);
            var curValue = infoEnumerator.Current.Value;
            curValue._obj.Set(renderInfo);                
            newDict.Add(infoEnumerator.Current.Key, new RenderInfoInt(renderInfo, curValue._obj, curValue._listener));
            j++;
            if (j >= cntImagesPerCol)
            {
                j = 0;
                i++;
            }
        }        

        _goToRender = new Dictionary<string, RenderInfoInt>(newDict);
        foreach (var listener in _goToRender)
        {            
            if (listener.Value._listener != null)
            {                
                listener.Value._listener.UpdateImage();
            }
        }
    }

}
