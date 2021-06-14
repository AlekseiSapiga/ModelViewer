using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Toggle _characterLookAtCamera;
    public BlickAnimation _applyButtonBlick;
    public Material[] _newMaterials;
    
    private string _imageId;
    private CharacterInvetory _characterInventory = null;
    private InventoryDataStore _dataStore = null;
    private InventoryCurrentChoiseHolder _currentChoise = null;
    private AnimationSwitcher _characterAnimationSwitcher = null;
    private CharacterMaterialSetter _characterMaterialSetter = null;

    void OnEnable()
    {
        _dataStore = new InventoryDataStore();
        _dataStore.Load();
        _currentChoise = new InventoryCurrentChoiseHolder(CharacterChoise._characterId, _dataStore, _inventorySO);
        
        var character = Array.Find(_charactersCollection._characters, ch => ch.GetId() == CharacterChoise._characterId);
        var newCharacter = Instantiate(character._prefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCharacter.transform.parent = _characterInstanceContainer;
        _characterMaterialSetter = newCharacter.GetComponentInChildren<CharacterMaterialSetter>();

        InitKineticRotation(newCharacter);

        InitCharacterRender(newCharacter, character);

        InitInventory(newCharacter);

        InitAnimationSwitcher(newCharacter);

        InitLookAtCameraSwitch(newCharacter);
    }


    private void InitCharacterRender(GameObject newCharacter, CharacterData characterDb)
    {
        var renderHelper = newCharacter.GetComponent<ViewPortSetter>();
        _imageId = newCharacter.GetInstanceID().ToString();        
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
        _characterInventory = newCharacter.GetComponent<CharacterInvetory>();

        foreach (var invCat in _inventoryCategories)
        {
            var categoryData = _inventorySO.GetCategory(invCat._id);
            if (categoryData != null)
            {                
                invCat._fill.Fill(invCat._id, categoryData._items, this, _characterInventory);
            }
        }

        

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
                        _characterInventory.Wear(new CharacterInventoryItem(itmFomDb._prefab, itm._category, itmFomDb.GetId()));
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

    private void InitLookAtCameraSwitch(GameObject newCharacter)
    {
        var lookAtComp = newCharacter.GetComponentInChildren<CharacterLookAt>();
        if (lookAtComp == null || _characterLookAtCamera == null)
        {
            return;
        }
        _characterLookAtCamera.onValueChanged.AddListener(delegate {
            lookAtComp.ikActive = _characterLookAtCamera.isOn;
        });
    }

    public void OnSelectItem(InventoryItem item, InventoryCategoryId categoryId)
    {        
        if (_characterInventory != null)
        {
            _characterInventory.Wear(new CharacterInventoryItem(item._prefab, categoryId, item.GetId()));
        }
        if (_currentChoise != null)
        {
            _currentChoise.OnSelectItem(item, categoryId);
            if (_applyButtonBlick != null)
            {
                _applyButtonBlick.AnimateBlick(_currentChoise.HasChanges());
            }
        }
        if (_characterMaterialSetter != null)
        {
            _characterMaterialSetter.UpdateMaterial();
        }
    }

    public void OnApplyClick()
    {
        if (_currentChoise != null)
        {
            _currentChoise.Apply();
        }
        if (_applyButtonBlick != null)
        {
            _applyButtonBlick.AnimateBlick(false);
        }
    }

    public void OnBackClick()
    {
        Renderer.Instance.Clear();
        SceneManager.LoadScene(0);
    }

    public void OnThermalViewClick()
    {
        if (_characterMaterialSetter == null || _newMaterials.Length < 1)
        {
            return;
        }
        _characterMaterialSetter.SetMaterial(_newMaterials[0]);
    }

    public void OnPhantomViewClick()
    {
        if (_characterMaterialSetter == null || _newMaterials.Length < 2)
        {
            return;
        }
        _characterMaterialSetter.SetMaterial(_newMaterials[1]);
    }

    public void OnOriginViewClick()
    {
        if (_characterMaterialSetter == null)
        {
            return;
        }
        _characterMaterialSetter.RestoreOriginal();
    }
}
