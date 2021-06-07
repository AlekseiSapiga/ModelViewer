using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Wearing
{
    public InventoryCategoryId _category;
    public GameObject _locator;
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
        RemoveInt(item._id, wearingsSettings);

        var newItem = Instantiate(item._prefab, new Vector3(0, 0, 0), Quaternion.identity);
        newItem.transform.SetParent(wearingsSettings._locator.transform);
        newItem.layer = wearingsSettings._locator.layer;
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
            for (int i = 0; i < wearingsSettings._locator.transform.childCount; i++)
            {
                transform = wearingsSettings._locator.transform.GetChild(i);
                GameObject.Destroy(transform.gameObject);
            }
            _currentWears.Remove(id);
        }
    }

    public bool CanWear(InventoryItem item, InventoryCategoryId categoryId)
    {
        return true;
    }
}
