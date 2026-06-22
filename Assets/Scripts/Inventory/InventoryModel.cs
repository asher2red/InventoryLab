using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;

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

    public bool AddItem(ItemData itemData, int count = 1)
    {
        if (itemData == null || count <= 0)
        {
            return false;
        }

        int remain = count;

        if (itemData.stackable)
        {
            for (int i = 0; i<items.Count; i++)
            {
                var item = items[i];

                if (item == null)
                {
                    continue;
                }

                if (item.data != itemData)
                {
                    continue;
                }

                if (item.count >= itemData.maxStack)
                {
                    continue;
                }

                int addable = itemData.maxStack - item.count;
                int added = Math.Min(addable, remain);

                item.count += added;
                remain -= added;

                if (remain <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }

        for (int i = 0; i<items.Count; i++)
        {
            if (items[i] != null)
            {
                continue;
            }

            int added = itemData.stackable ? Math.Min(remain, itemData.maxStack) : 1;

            items[i] = new InventoryItem
            {
                data = itemData,
                count = added
            };

            remain -= added;

            if (remain <= 0)
            {
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        OnInventoryChanged?.Invoke();

        return remain <= 0;
    }

    public bool RemoveItem(ItemData itemData, int count = 1)
    {
        if (itemData == null || count <= 0)
        {
            return false;
        }

        int remain = count;

        for (int i = items.Count - 1; i>=0; i--)
        {
            var item = items[i];

            if (item == null)
            {
                continue;
            }

            if (item.data != itemData)
            {
                continue;
            }

            int removed = Math.Min(item.count, remain);

            item.count -= removed;
            remain -= removed;

            if (item.count <= 0)
            {
                items[i] = null;
            }

            if (remain <= 0)
            {
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        OnInventoryChanged?.Invoke();

        return false;
    }
}