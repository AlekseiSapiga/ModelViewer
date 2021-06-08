using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class InventoryDataStore
{
    private const string _fileName = "/save.dat";
    private Characters _characters = null;

    [System.Serializable]
    public class CategoryAndItem
    {
        public InventoryCategoryId _category;
        public string _item;
        public CategoryAndItem() { }
        public CategoryAndItem(InventoryCategoryId category, string item) { _category = category; _item = item; }
    }

    [System.Serializable]
    class Character
    {
        public string _id;
        public List<CategoryAndItem> _categoryAndItem;
        public Character() { }

        public Character(string characterId, InventoryCategoryId category, string item)
        {
            _id = characterId;
            Set(category, item);
        }

        public void Set(InventoryCategoryId category, string item)
        {
            var newCategoryAndItem = new CategoryAndItem(category, item);
            if (_categoryAndItem == null)
            {
                _categoryAndItem = new List<CategoryAndItem>();
                _categoryAndItem.Add(newCategoryAndItem);
                return;
            }
            _categoryAndItem.RemoveAll(ct => ct._category == category);
            _categoryAndItem.Add(newCategoryAndItem);
        }
    }

    [System.Serializable]
    class Characters
    {
        public List<Character> _list;
    }

    public List<CategoryAndItem> Get(string characterId)
    {
        if (_characters == null)
        {
            return null;
        }
        var character = _characters._list.Find(chr => chr._id == characterId);
        if (character == null)
        {
            return null;
        }
        return character._categoryAndItem;
    }

    public void Set(string characterId, InventoryCategoryId category, string item)
    {
        if (_characters == null)
        {
            _characters = new Characters();
            _characters._list = new List<Character>();
            _characters._list.Add(new Character(characterId,  category, item));
            return;
        }
        
        var character = _characters._list.Find(chr => chr._id == characterId);
        if (character == null)
        {
            _characters._list.Add(new Character(characterId, category, item));
            return;
        }

        character.Set(category, item);
    }

    public void Load()
    {
        string destination = Application.persistentDataPath + _fileName;
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenRead(destination);
        }
        else
        {
            _characters = null;
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        _characters = (Characters)bf.Deserialize(file);
        file.Close();
    }

    public void Save()
    {
        if (_characters == null)
        {
            return;
        }
        string destination = Application.persistentDataPath + _fileName;
        FileStream file;

        if (File.Exists(destination))
        {
            file = File.OpenWrite(destination);
        }
        else
        {
            file = File.Create(destination);
        }            
        
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, _characters);
        file.Close();
    }




}
