using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image icon;

    [SerializeField] private TMP_Text countText;

    [SerializeField] private Button button;

    private int slotIndex;

    private System.Action<int> onClick;
    private System.Action<int, int> onDrop;

    public void Initialize(int index, System.Action<int> click, System.Action<int, int> drop)
    {
        slotIndex = index;
        onClick = click;
        onDrop = drop;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Begin Drag: {slotIndex}");

        InventoryDragContext.DragIndex = slotIndex;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End Drag: {slotIndex}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"Drop: {slotIndex}");

        int from = InventoryDragContext.DragIndex;
        int to = slotIndex;

        onDrop?.Invoke(from, to);
    }
}