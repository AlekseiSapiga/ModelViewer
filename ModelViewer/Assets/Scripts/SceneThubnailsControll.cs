using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneThubnailsControll : MonoBehaviour
{
    public CharactersCollection _charactersCollection;
    public RenderTexture _texture;
    public GameObject _widgetPrefab;
    public GameObject _scroller;

    void OnEnable()
    {
        Renderer.Instance.Init(_texture, 256);
        foreach (var character in _charactersCollection._characters)
        {
            var newCharacter = Instantiate(character._prefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCharacter.transform.parent = transform;
            var renderHelper = newCharacter.GetComponent<ViewPortSetter>();
            var imageId = newCharacter.name;
            
            ThubnailWidget newWidget = null;
            if (_scroller)
            {
                var newWidgetObj = Instantiate(_widgetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newWidgetObj.transform.SetParent(_scroller.transform);
                newWidget = newWidgetObj.GetComponent<ThubnailWidget>();
                newWidget.SetTitle(character._title);
                newWidget.SetImageId(imageId);
            }

            if (renderHelper)
            {
                Renderer.Instance.Register(imageId, renderHelper, newWidget);
            }

        }
    }
    
}
