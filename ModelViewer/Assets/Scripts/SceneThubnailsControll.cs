using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneThubnailsControll : MonoBehaviour
{
    public CharactersCollection _charactersCollection;
    public RenderTexture _texture;
    public GameObject _widgetPrefab;
    public GameObject _scrollerContent;
    public ScrollRect _scrollerRect; 

    void OnEnable()
    {
        Renderer.Instance.Init(_texture, 256);
        foreach (var character in _charactersCollection._characters)
        {
            var newCharacter = Instantiate(character._prefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCharacter.transform.parent = transform;
            var renderHelper = newCharacter.GetComponent<ViewPortSetter>();
            var imageId = newCharacter.GetInstanceID().ToString();
            Debug.Log("Create " + newCharacter.name + " id "+ imageId);
            ThubnailWidget newWidget = null;
            if (_scrollerContent)
            {
                var newWidgetObj = Instantiate(_widgetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newWidgetObj.transform.SetParent(_scrollerContent.transform);
                newWidget = newWidgetObj.GetComponent<ThubnailWidget>();
                newWidget.SetTitle(character._title);
                newWidget.SetImageId(imageId);
                var widgetVisibilityCntrl = newWidget.GetComponent<WidgetVisibilityControll>();
                if (widgetVisibilityCntrl)
                {
                    widgetVisibilityCntrl.Init(_scrollerRect, imageId);
                }                
            }

            if (renderHelper)
            {
                Renderer.Instance.Register(imageId, renderHelper, newWidget);
            }

        }
    }
    
}
