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
        public RenderInfo _info { get; set; }
        public ViewPortSetter _obj { get; private set; }
        public IUpdateImageListener _listener { get; private set; }
        
        public RenderInfoInt() { }
        public RenderInfoInt(RenderInfo info, ViewPortSetter obj, IUpdateImageListener listener)
        {
            _info = info;
            _obj = obj;
            _listener = listener;            
        }
    }

    private static Renderer instance = null;
    private Dictionary<string, RenderInfoInt> _goToRender;
    private Dictionary<string, RenderInfoInt> _goExcluded = new Dictionary<string, RenderInfoInt>();
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
        if (!_inited)
        {
            Debug.Log("SetVisible return " + id);
            return;
        }
        if (value)
        {
            if (_goToRender.ContainsKey(id) || !_goExcluded.ContainsKey(id))
            {
                UpdateInfos();
                return;
            }
            _goToRender.Add(id, _goExcluded[id]);
            _goExcluded.Remove(id);
        }
        else
        {
            if (_goExcluded.ContainsKey(id) || !_goToRender.ContainsKey(id))
            {
                UpdateInfos();
                return;
            }
            _goExcluded.Add(id, _goToRender[id]);
            _goToRender.Remove(id);
        }

        
        UpdateInfos();
    }

    public void Clear()
    {
        _goToRender.Clear();
        _goExcluded.Clear();
    }

    public RenderInfo GetRenderInfo(string id)
    {
        if (!_goToRender.ContainsKey(id))
        {            
            return new RenderInfo();
        }
        return _goToRender[id]._info;
    }

    private void UpdateInfos()
    {
        var cnt = _goToRender.Count;
        if (!_inited || cnt == 0)
        {
            return;
        }        

        int cntImagesPerCol = (int)Math.Ceiling(Math.Sqrt(cnt));
        //Debug.Log("cntImagesPerCol " + cntImagesPerCol + " cnt = " + cnt);
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
        int renderIndex = 0;
        while (infoEnumerator.MoveNext())
        {
            var curValue = infoEnumerator.Current.Value;           
            var rect = new Rect(shift * j, shift * i, shift, shift);

            var renderInfo = new RenderInfo(rect, _texture, renderIndex++);
            
            curValue._obj.Set(renderInfo);
            curValue._info = renderInfo;
            newDict.Add(infoEnumerator.Current.Key, curValue);// new RenderInfoInt(renderInfo, curValue._obj, curValue._listener));
            j++;
            if (j >= cntImagesPerCol)
            {
                j = 0;
                i++;
            }
        }

        foreach(var ex in _goExcluded)
        {
            ex.Value._obj.Set(new RenderInfo());
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
