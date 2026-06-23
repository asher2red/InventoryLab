using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<ItemData> items;

    private Dictionary<int, ItemData> itemMap;

    private void Awake()
    {
        itemMap = new();

        foreach (var item in items)
        {
            itemMap[item.id] = item;
        }
    }

    public ItemData GetItem(int id)
    {
        itemMap.TryGetValue(id, out var item);

        return item;
    }
}