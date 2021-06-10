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


public class CharacterPreviewController : MonoBehaviour, IInventoryItemSelect, IUIAnimationSwitch
{
    public CharactersCollection _charactersCollection;
    public RenderTexture _texture;
    public Transform _characterInstanceContainer;
    public ThubnailWidget _previewWidget;
    public InventoryDB _inventorySO;
    public InventoryCategoriesBinder[] _inventoryCategories;
    public KineticRotationDB _kineticRotationSO;
    public RectTransform _rectToHandleRotationTouch;
    public UIAnimationSwitcher _animationSwitcherUI;

    private string _imageId;
    private CharacterInvetory _characterInventory = null;
    private InventoryDataStore _dataStore = null;
    private InventoryCurrentChoiseHolder _currentChoise = null;
    private AnimationSwitcher _characterAnimationSwitcher = null;

    void OnEnable()
    {
        _dataStore = new InventoryDataStore();
        _dataStore.Load();
        _currentChoise = new InventoryCurrentChoiseHolder(CharacterChoise._characterId, _dataStore, _inventorySO);

        Renderer.Instance.Init(_texture, 256);
        var character = Array.Find(_charactersCollection._characters, ch => ch.GetId() == CharacterChoise._characterId);
        var newCharacter = Instantiate(character._prefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCharacter.transform.parent = _characterInstanceContainer;

        InitKineticRotation(newCharacter);

        InitCharacterRender(newCharacter, character);

        InitInventory(newCharacter);

        InitAnimationSwitcher(newCharacter);
    }


    private void InitCharacterRender(GameObject newCharacter, CharacterData characterDb)
    {
        var renderHelper = newCharacter.GetComponent<ViewPortSetter>();
        _imageId = newCharacter.GetInstanceID().ToString();
        Debug.Log("Create " + newCharacter.name + " id " + _imageId);
        InitImageWidget(characterDb);
        if (renderHelper)
        {
            Renderer.Instance.Register(_imageId, renderHelper, _previewWidget);
        }        
    }

    private void InitImageWidget(CharacterData characterDb)
    {
        if (!_previewWidget)
        {
            return;
        }
        
        _previewWidget.SetTitle(characterDb._title);
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

        if (_characterInventory != null)
        {
            var saved = _dataStore.Get(CharacterChoise._characterId);
            if (saved != null)
            {
                foreach (var itm in saved)
                {
                    var itmFomDb = _inventorySO.GetItem(itm._category, itm._item);
                    if (itmFomDb != null)
                    {
                        _characterInventory.Wear(new CharacterInventoryItem(itmFomDb._prefab, itm._category, itmFomDb.GetId()));// itm._category, itmFomDb);
                    }
                }
            }
            
        }
    }

    private void InitKineticRotation(GameObject newCharacter)
    {
        var modelHolder = newCharacter.GetComponent<CharacterModelHolder>();
        if (modelHolder)
        {
            var kineticRot = modelHolder._characterModel.gameObject.AddComponent<KineticRotation>();
            if (_kineticRotationSO != null)
            {
                kineticRot.Init(_kineticRotationSO, _rectToHandleRotationTouch);
            }
        }
    }

    private void InitAnimationSwitcher(GameObject newCharacter)
    {
        if (_animationSwitcherUI != null)
        {
            _animationSwitcherUI.Init(this);
        }
        _characterAnimationSwitcher = newCharacter.GetComponent<AnimationSwitcher>();        
    }

    public void OnAnimationSelected(AnimationTypes type)
    {
        if (_characterAnimationSwitcher != null)
        {
            _characterAnimationSwitcher.StartAnimation(type);
        }
    }

    public void OnSelectItem(InventoryItem item, InventoryCategoryId categoryId)
    {
        Debug.Log("ON OnSelectItem " + categoryId._id + " | " + item._icon.name);
        if (_characterInventory != null)
        {
            _characterInventory.Wear(new CharacterInventoryItem(item._prefab, categoryId, item.GetId()));
        }
        if (_currentChoise != null)
        {
            _currentChoise.OnSelectItem(item, categoryId);
        }
    }

    public void OnApplyClick()
    {
        if (_currentChoise != null)
        {
            _currentChoise.Apply();
        }
    }

    public void OnBackClick()
    {

    }
}
