using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int id;

    public string itemName;

    [TextArea]
    public string description;

    public Sprite icon;

    public bool stackable = true;

    public int maxStack = 99;
}