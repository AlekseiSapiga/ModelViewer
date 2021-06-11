using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCurrentChoiseHolder : IInventoryItemSelect
{
    private Dictionary<InventoryCategoryId, InventoryItem> _currentSelection;
    private Dictionary<InventoryCategoryId, InventoryItem> _initSelection;
    private string _characterId;
    private InventoryDataStore _dataStore;

    public InventoryCurrentChoiseHolder(string characterId, InventoryDataStore dataStore, InventoryDB inventoryDB)
    {
        _characterId = characterId;
        _dataStore = dataStore;
        _currentSelection = new Dictionary<InventoryCategoryId, InventoryItem>();
        _initSelection = new Dictionary<InventoryCategoryId, InventoryItem>();
        var saved = _dataStore.Get(_characterId);
        if (saved != null)
        {
            foreach (var itm in saved)
            {
                var itmFomDb = inventoryDB.GetItem(itm._category, itm._item);
                if (itmFomDb != null)
                {
                    _currentSelection.Add(itm._category, itmFomDb);
                    _initSelection.Add(itm._category, itmFomDb);
                }
            }
        }
        
    }

    public void Apply()
    {
        if (_currentSelection.Count > 0)
        {
            _initSelection.Clear();
            foreach (var pair in _currentSelection)
            {
                _dataStore.Set(_characterId, pair.Key, pair.Value.GetId());
                _initSelection.Add(pair.Key, pair.Value);
            }
            _dataStore.Save();
        }
    }

    public void Reset()
    {
        _currentSelection.Clear();
    }

    public void OnSelectItem(InventoryItem item, InventoryCategoryId categoryId)
    {
        _currentSelection[categoryId] = item;
    }

    public bool HasChanges()
    {        
        if (_initSelection.Count != _currentSelection.Count)
        {
            return true;
        }

        bool equal = true;
        foreach (var pair in _currentSelection)
        {
            InventoryItem value;
            if (_initSelection.TryGetValue(pair.Key, out value))
            {
                if (value.GetId() != pair.Value.GetId())
                {
                    equal = false;
                    break;
                }
            }
            else
            {
                equal = false;
                break;
            }
        }
        return !equal;
    }

}
