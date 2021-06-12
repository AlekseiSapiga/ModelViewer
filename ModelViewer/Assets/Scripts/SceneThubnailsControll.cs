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
    public InventoryDB _inventorySO;

    private InventoryDataStore _dataStore = null;

    void OnEnable()
    {
        _dataStore = new InventoryDataStore();
        _dataStore.Load();

        Renderer.Instance.Init(_texture, 256);
        foreach (var character in _charactersCollection._characters)
        {
            var newCharacter = Instantiate(character._prefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCharacter.transform.parent = transform;
            var renderHelper = newCharacter.GetComponent<ViewPortSetter>();
            var imageId = newCharacter.GetInstanceID().ToString();
            Debug.Log("Create " + newCharacter.name + " id " + imageId);
            ThubnailWidget newWidget = null;
            if (_scrollerContent)
            {
                var newWidgetObj = Instantiate(_widgetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newWidgetObj.transform.SetParent(_scrollerContent.transform);
                newWidget = newWidgetObj.GetComponent<ThubnailWidget>();
                newWidget.Init(character._title, imageId, character.GetId());

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

            InitCharacterInventory(newCharacter, character.GetId());

        }
    }

    private void InitCharacterInventory(GameObject newCharacter, string characterId)
    {
        var characterInventory = newCharacter.GetComponent<CharacterInvetory>();

        if (characterInventory != null)
        {
            var saved = _dataStore.Get(characterId);
            if (saved != null)
            {
                foreach (var itm in saved)
                {
                    var itmFomDb = _inventorySO.GetItem(itm._category, itm._item);
                    if (itmFomDb != null)
                    {
                        characterInventory.Wear(new CharacterInventoryItem(itmFomDb._prefab, itm._category, itmFomDb.GetId()));
                    }
                }
            }

        }
    }
    
}
