using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

[System.Serializable]
public class CharacterData
{
    public GameObject _prefab;
    public string _title;
    [SerializeField]
    [ReadOnly]
    private string _id;

    public string GetId() { return _id; }
}


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharactersCollection", order = 1)]
public class CharactersCollection : ScriptableObject
{
    public CharacterData[] _characters;

    private void OnValidate()
    {
#if UNITY_EDITOR
        HashSet<string> _idsNew = new HashSet<string>();

        foreach (var item in _characters)
        {
            if (item.GetId() == "" || _idsNew.Contains(item.GetId()))
            {
                typeof(CharacterData).GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, Guid.NewGuid().ToString());
                UnityEditor.EditorUtility.SetDirty(this);
            }
            _idsNew.Add(item.GetId());
        }

#endif
    }
}
