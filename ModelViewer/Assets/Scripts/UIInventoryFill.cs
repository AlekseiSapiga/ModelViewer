using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryFill : MonoBehaviour
{    
    public RectTransform _container;
    public GameObject _widgetPrefab;

    private InventoryCategoryId _categoryId;

    public void Fill(InventoryCategoryId categoryId, InventoryItem[] items, IInventoryItemSelect selectCallback, ICharacterCanWear canAccept)
    {
        _categoryId = categoryId;
        foreach(var item in items)
        {
            if (canAccept != null && !canAccept.CanWear(item, categoryId))
            {
                continue;
            }
            var newWidgetObj = Instantiate(_widgetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newWidgetObj.transform.SetParent(_container);
            var widget = newWidgetObj.GetComponent<InventoryItemWidget>();
            widget.Set(item, delegate { selectCallback.OnSelectItem(item, categoryId); });
        }
    }
}
