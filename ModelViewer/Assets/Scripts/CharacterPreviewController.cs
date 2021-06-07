using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInventoryItemSelect
{
    void OnSelectItem(InventoryItem item, InventoryCategoryId categoryId);
}


[System.Serializable]
public class InventoryCategoriesBinder
{
    public InventoryCategoryId _id;
    public UIInventoryFill _fill;
}


public class CharacterPreviewController : MonoBehaviour, IInventoryItemSelect
{
    public CharactersCollection _charactersCollection;
    public RenderTexture _texture;
    public Transform _characterInstanceContainer;
    public ThubnailWidget _previewWidget;
    public InventoryDB _inventorySO;
    public InventoryCategoriesBinder[] _inventoryCategories;

    private string _imageId;
    private CharacterInvetory _characterInventory = null;
    void Start()
    {
        Renderer.Instance.Init(_texture, 256);
        var character = _charactersCollection._characters[CharacterChoise._characterId];
        var newCharacter = Instantiate(character._prefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCharacter.transform.parent = _characterInstanceContainer;

        InitCharacterRender(newCharacter);

        InitInventory(newCharacter);
    }


    private void InitCharacterRender(GameObject newCharacter)
    {
        var renderHelper = newCharacter.GetComponent<ViewPortSetter>();
        _imageId = newCharacter.GetInstanceID().ToString();
        Debug.Log("Create " + newCharacter.name + " id " + _imageId);
        InitImageWidget();
        if (renderHelper)
        {
            Renderer.Instance.Register(_imageId, renderHelper, _previewWidget);
        }        
    }

    private void InitImageWidget()
    {
        if (!_previewWidget)
        {
            return;
        }
        var character = _charactersCollection._characters[CharacterChoise._characterId];
        _previewWidget.SetTitle(character._title);
        _previewWidget.SetImageId(_imageId);  
    }

    private void InitInventory(GameObject newCharacter)
    {
        if (_inventoryCategories == null || _inventorySO == null)
        {
            return;
        }
        
        foreach (var invCat in _inventoryCategories)
        {
            var categoryData = _inventorySO.GetCategory(invCat._id);
            if (categoryData != null)
            {                
                invCat._fill.Fill(invCat._id, categoryData._items, this, null);
            }
        }

        _characterInventory = newCharacter.GetComponent<CharacterInvetory>();
    }

    public void OnSelectItem(InventoryItem item, InventoryCategoryId categoryId)
    {
        Debug.Log("ON OnSelectItem " + categoryId._id + " | " + item._icon.name);
        if (_characterInventory != null)
        {
            _characterInventory.Wear(new CharacterInventoryItem(item._prefab, categoryId));
        }
    }

}
