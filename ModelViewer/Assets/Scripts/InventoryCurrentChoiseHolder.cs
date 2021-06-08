using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCurrentChoiseHolder : IInventoryItemSelect
{
    private Dictionary<InventoryCategoryId, InventoryItem> _currentSelection;
    private string _characterId;
    private InventoryDataStore _dataStore;

    public InventoryCurrentChoiseHolder(string characterId, InventoryDataStore dataStore, InventoryDB inventoryDB)
    {
        _characterId = characterId;
        _dataStore = dataStore;
        _currentSelection = new Dictionary<InventoryCategoryId, InventoryItem>();
        var saved = _dataStore.Get(_characterId);
        if (saved != null)
        {
            foreach (var itm in saved)
            {
                var itmFomDb = inventoryDB.GetItem(itm._category, itm._item);
                if (itmFomDb != null)
                {
                    _currentSelection.Add(itm._category, itmFomDb);
                }
            }
        }
        
    }

    public void Apply()
    {
        if (_currentSelection.Count > 0)
        {
            foreach (var pair in _currentSelection)
            {
                _dataStore.Set(_characterId, pair.Key, pair.Value.GetId());
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

}
