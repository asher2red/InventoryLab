using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI slotPrefab;

    [SerializeField] private Transform slotRoot;

    [SerializeField] private InventoryDetailUI detailUI;

    [SerializeField] private List<ItemData> testItems;

    [SerializeField] private int slotCount = 20;

    [SerializeField] ItemDatabase itemDatabase;

    private readonly List<InventorySlotUI> slots = new();

    private InventoryModel model;

    private void Start()
    {
        model = new InventoryModel(slotCount);

        CreateSlots();

        model.OnInventoryChanged += Refresh;
        model.OnSelectedSlotChanged += OnSelectedSlotChanged;

        model.AddItem(testItems[0], 1);
        model.AddItem(testItems[1], 30);
        model.AddItem(testItems[2], 3);
        model.DebugSetItem(3, testItems[1], 50);
        model.DebugSetItem(4, testItems[1], 50);

        Refresh();
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.OnInventoryChanged -= Refresh;
            model.OnSelectedSlotChanged += OnSelectedSlotChanged;
        }
    }

    private void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slot.Initialize(i, model.SelectSlot, model.HandleDrop);

            slots.Add(slot);
        }
    }

    public void SaveInventory()
    {
        InventorySaveSystem.Save(model.Export());
    }

    public void LoadnInventory()
    {
        var data = InventorySaveSystem.Load();

        if (data == null) return;

        model.Import(data, itemDatabase);
    }

    private void Refresh()
    {
        for (int i = 0; i<slots.Count; i++)
        {
            slots[i].Bind(model.Items[i]);
        }
    }

    private void OnSelectedSlotChanged(int index)
    {
        detailUI.Bind(model.Items[index]);
    }

    
    public void AddPotion()
    {
        model.AddItem(testItems[1], 1);
    }

    public void RemovePotion()
    {
        model.RemoveItem(testItems[1], 1);
    }

    public void TestMove()
    {
        model.MoveItem(0, 10);
    }

    public void TestSwap()
    {
        model.SwapItem(0, 1);
    }

    public void TestSplit()
    {
        int selected = model.SelectedIndex;

        if (selected < 0) return;

        model.SplitItem(selected, 10);
    }
}