using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThubnailWidget : MonoBehaviour, IUpdateImageListener
{
    public Text _title;
    public RawImage _image;
    private string _imageId;
    
    public void Init(string title, string imageid, string characterId)
    {
        SetTitle(title);
        SetImageId(imageid);

        GetComponent<Button>().onClick.AddListener(delegate {
            CharacterChoise.LoadCharacterPreviewScene(characterId);
        });
    }

    public void SetTitle(string title)
    {
        _title.text = title;
    }
    
    public void SetImageId(string id)
    {
        _imageId = id;
        _title.text += id;
    }

    public void UpdateImage()
    {       
        var renderInfo = Renderer.Instance.GetRenderInfo(_imageId);        
        if (!renderInfo.IsValid())
        {
            return;
        }        
        _image.texture = renderInfo._texture;
        _image.uvRect = renderInfo._uvRect;
    }
}
