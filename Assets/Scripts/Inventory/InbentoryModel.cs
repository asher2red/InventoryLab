using System;
using System.Collections.Generic;

public class InventoryModel
{
    public event Action OnInventoryChanged;

    public IReadOnlyList<InventoryItem> Items => items;

    private readonly List<InventoryItem> items;

    public InventoryModel(int slotCount)
    {
        items = new List<InventoryItem>(slotCount);

        for (int i = 0; i<slotCount; i++)
        {
            items.Add(null);
        }
    }

    public void SetItem(int index, InventoryItem item)
    {
        if (index < 0 || index >= items.Count)
        {
            return;
        }

        items[index] = item;

        OnInventoryChanged?.Invoke();
    }
}