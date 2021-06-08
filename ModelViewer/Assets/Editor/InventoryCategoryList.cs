using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryCategoryList", menuName = "ScriptableObjects/InventoryCategoryList", order = 1)]
public class InventoryCategoryList : ScriptableObject
{
    public string[] _categories;
}
