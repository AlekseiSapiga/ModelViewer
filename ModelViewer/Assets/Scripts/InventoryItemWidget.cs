using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnSelectInventoryItem();

public class InventoryItemWidget : MonoBehaviour
{
    public Image _image;
    public Button _button;
    
    public void Set(InventoryItem item, OnSelectInventoryItem callback )
    {
        if (_image)
        {
            _image.sprite = item._icon;
        }
        if (_button)
        {
            _button.onClick.AddListener(() => callback());
        }
    }    
}
