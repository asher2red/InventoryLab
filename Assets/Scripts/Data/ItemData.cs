using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int id;

    public string itemName;

    [TextArea]
    public string description;

    public AssetReferenceSprite iconReference;

    public bool stackable = true;

    public int maxStack = 99;
}