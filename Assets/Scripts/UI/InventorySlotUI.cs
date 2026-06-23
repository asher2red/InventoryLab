using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    [SerializeField] private TMP_Text countText;

    [SerializeField] private Button button;

    private int slotIndex;

    private System.Action<int> onClick;

    public void Initialize(int index, System.Action<int> callback)
    {
        slotIndex = index;
        onClick = callback;

        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

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

    private void OnClick()
    {
        onClick?.Invoke(slotIndex);
    }
}