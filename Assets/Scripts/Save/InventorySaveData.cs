using System;
using System.Collections.Generic;

[Serializable]
public class InventorySaveData
{
    public int version = 1;

    public List<InventorySlotSaveData> items = new();
}