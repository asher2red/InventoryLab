using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    [SerializeField] private TMP_Text countText;

    public void Bind(InventoryItem item)
    {
        bool hasItem = item != null && item.data != null;

        icon.enabled = hasItem;

        if (!hasItem)
        {
            countText.text = "";
            return;
        }

        icon.sprite = item.data.icon;

        countText.text = item.count > 1 ? item.count.ToString() : "";
    }
}