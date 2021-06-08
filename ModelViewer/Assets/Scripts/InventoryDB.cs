using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ReadOnlyAttribute : PropertyAttribute { }

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

    [SerializeField]
    [ReadOnly]
    private string _id;

    public string GetId() { return _id; }
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

    public InventoryItem GetItem(InventoryCategoryId categoryId, string itemId)
    {
        var category = GetCategory(categoryId);
        if (category != null)
        {
            return Array.Find(category._items, itm => itm.GetId() == itemId);
        }
        return null;
    }

    private void OnValidate()
    {
        #if UNITY_EDITOR
        HashSet<string> _idsNew = new HashSet<string>();

        foreach (var cat in _categories)
        {
            foreach (var item in cat._items)
            {
                if (item.GetId() == "" || _idsNew.Contains(item.GetId()))
                {
                    typeof(InventoryItem).GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, Guid.NewGuid().ToString());
                    UnityEditor.EditorUtility.SetDirty(this);
                }
                _idsNew.Add(item.GetId());
            }
        }

        #endif
    }
}