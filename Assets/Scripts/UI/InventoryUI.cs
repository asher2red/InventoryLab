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

        CreateTestData();

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

    private void CreateTestData()
    {
        for (int i = 0; i < testItems.Count; i++)
        {
            model.SetItem(i, new InventoryItem
            {
                data = testItems[i],
                count = Random.Range(1, 20)
            });
        }
    }

    private void Refresh()
    {
        for (int i = 0; i<slots.Count; i++)
        {
            slots[i].Bind(model.Items[i]);
        }
    }
}