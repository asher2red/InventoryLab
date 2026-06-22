using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI slotPrefab;

    [SerializeField] private Transform slotRoot;

    [SerializeField] private List<ItemData> testItems;

    [SerializeField] private int slotCount = 20;

    private readonly List<InventorySlotUI> slots = new();

    private InventoryModel model;

    private void Start()
    {
        model = new InventoryModel(slotCount);

        CreateSlots();

        model.OnInventoryChanged += Refresh;

        model.AddItem(testItems[0], 1);
        model.AddItem(testItems[1], 100);
        model.AddItem(testItems[2], 3);

        Refresh();
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnInventoryChanged -= Refresh;
        }
    }

    private void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);

            slots.Add(slot);
        }
    }

    private void Refresh()
    {
        for (int i = 0; i<slots.Count; i++)
        {
            slots[i].Bind(model.Items[i]);
        }
    }

    public void AddPotion()
    {
        model.AddItem(testItems[1], 1);
    }

    public void RemovePotion()
    {
        model.RemoveItem(testItems[1], 1);
    }
}