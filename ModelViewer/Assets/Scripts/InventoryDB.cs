using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InventoryCategoryId
{
    public string _id;
    public static bool operator ==(InventoryCategoryId b1, InventoryCategoryId b2)
    {
        return b1.Equals(b2);
    }

    public static bool operator !=(InventoryCategoryId b1, InventoryCategoryId b2)
    {
        return !(b1 == b2);
    }

    public override bool Equals(object obj)
    {
        return _id == ((InventoryCategoryId)obj)._id;
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }
}


public class CharacterInventoryItem
{
    public GameObject _prefab { get; private set; }
    public InventoryCategoryId _id { get; private set; }
    public CharacterInventoryItem() { }
    public CharacterInventoryItem(GameObject prefab, InventoryCategoryId id)
    {
        _prefab = prefab;
        _id = id;
    }
}

[System.Serializable]
public class InventoryItem
{
    public GameObject _prefab;    
    public Sprite _icon;
}

[System.Serializable]
public class InventoryCategory
{
    public InventoryCategoryId _id;
    public string _title;
    public InventoryItem[] _items;
}

[CreateAssetMenu(fileName = "InventoryDB", menuName = "ScriptableObjects/InventoryDB", order = 1)]
public class InventoryDB : ScriptableObject
{
    public InventoryCategory[] _categories;

    public InventoryCategory GetCategory(InventoryCategoryId id)
    {
        return Array.Find(_categories, cat=> cat._id == id);
    }
}