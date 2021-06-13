using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ItemIdLocator
{
    public string _id;
    public GameObject _locator;
}


[System.Serializable]
public class Wearing
{
    public InventoryCategoryId _category;
    public List<ItemIdLocator> _suitableItems;
}

public interface ICharacterCanWear
{
    bool CanWear(InventoryItem item, InventoryCategoryId categoryId);
}


public class CharacterInvetory : MonoBehaviour, ICharacterCanWear
{
    public Wearing[] _wearings;
    private HashSet<InventoryCategoryId> _currentWears = new HashSet<InventoryCategoryId>();

    public void Wear(CharacterInventoryItem item)
    {
        var wearingsSettings = Array.Find(_wearings, wr => wr._category == item._id);
        if (wearingsSettings == null)
        {
            return;
        }
        var itemLocator = wearingsSettings._suitableItems.Find(itm => itm._id == item._itemId);
        if (itemLocator == null || itemLocator._locator == null)
        {
            return;
        }
        var locator = itemLocator._locator;

        RemoveInt(item._id, wearingsSettings);
        Debug.Log("Before Instantiate");
        var newItem = Instantiate(item._prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log("After Instantiate");
        newItem.transform.parent = locator.transform;
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;
        newItem.transform.localScale = Vector3.one;
        newItem.layer = locator.layer;
        foreach (Transform child in newItem.transform)
        {
            child.gameObject.layer = locator.layer;
        }

        _currentWears.Add(item._id);
    }

    public void Remove(InventoryCategoryId id)
    {
        var wearingsSettings = Array.Find(_wearings, wr => wr._category == id);
        if (wearingsSettings == null)
        {
            return;
        }
        RemoveInt(id, wearingsSettings);
    }

    private void RemoveInt(InventoryCategoryId id, Wearing wearingsSettings)
    {
        if (_currentWears.Contains(id))
        {
            Transform transform;
            foreach (var itemLocator in wearingsSettings._suitableItems)
            {
                for (int i = 0; i < itemLocator._locator.transform.childCount; i++)
                {
                    transform = itemLocator._locator.transform.GetChild(i);
                    GameObject.Destroy(transform.gameObject);
                }
            }
            _currentWears.Remove(id);
        }
    }

    public bool CanWear(InventoryItem item, InventoryCategoryId categoryId)
    {
        if (_wearings == null)
        {
            return false;
        }
        var wearingsSettings = Array.Find(_wearings, wr => wr._category == categoryId);
        if (wearingsSettings == null)
        {
            return false;
        }
        return wearingsSettings._suitableItems.Find(itm => itm._id == item.GetId()) != null;
    }
}
