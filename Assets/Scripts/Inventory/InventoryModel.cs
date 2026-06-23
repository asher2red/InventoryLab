using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class InventoryModel
{
    public event Action OnInventoryChanged;
    public event Action<int> OnSelectedSlotChanged;

    public IReadOnlyList<InventoryItem> Items => items;

    private readonly List<InventoryItem> items;

    public int SelectedIndex { get; private set; } = -1;

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

    public InventoryItem GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return null;
        }

        return items[index];
    }

    public bool MoveItem(int from, int to)
    {
        if (from == to)
        {
            return false;
        }

        if (!IsValidIndex(from))
        {
            return false;
        }

        if (!IsValidIndex(to))
        {
            return false;
        }

        if (items[from] == null)
        {
            return false;
        }

        if (items[to] != null)
        {
            return false;
        }

        items[to] = items[from];
        items[from] = null;

        OnInventoryChanged!.Invoke();

        return true;
    }

    public bool SwapItem(int a, int b)
    {
        if (a == b)
        {
            return false;
        }

        if (!IsValidIndex(a))
        {
            return false;
        }

        if (!IsValidIndex(b))
        {
            return false;
        }

        (items[a], items[b]) = (items[b], items[a]);

        OnInventoryChanged!.Invoke();

        return true;
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return;
        }

        if (index == SelectedIndex)
        {
            return;
        }

        SelectedIndex = index;

        OnSelectedSlotChanged?.Invoke(index);
    }

    public void HandleDrop(int from, int to)
    {
        if (from == to)
        {
            return;
        }

        if (GetItem(to) == null)
        {
            MoveItem(from, to);
        }
        else
        {
            SwapItem(from, to);
        }
    }

    public void import(InventorySaveData saveData, ItemDatabase database)
    {
        for (int i = 0; i<items.Count; i++)
        {
            items[i] = null;
        }

        foreach (var entry in saveData.items)
        { 
            items[entry.slotIndex] = new InventoryItem
            {
                data = database.GetItem(entry.itemId),
                count = entry.count
            };
        }

        OnInventoryChanged?.Invoke();
    }

    public InventorySaveData Export()
    {
        InventorySaveData saveData = new();

        for (int i = 0; i<items.Count; i++)
        {
            var item = items[i];

            if (item == null) continue;

            saveData.items.Add(
                new InventorySlotSaveData
                {
                    slotIndex = i,
                    itemId = item.data.id,
                    count = item.count
                }
            );
        }

        return saveData;
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

    private bool IsValidIndex(int index)
    {
        return index >= 0 &&
            index < items.Count;
    }
}