using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public GameObject _prefab;
    public string _title;
}


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharactersCollection", order = 1)]
public class CharactersCollection : ScriptableObject
{
    public CharacterData[] _characters;
}
