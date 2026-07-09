using TMPro;
using UnityEngine;

public class InventoryDetailUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text countText;

    public void Bind(InventoryItem item)
    {
        if (item == null)
        {
            nameText.text = "";
            descriptionText.text = "";
            countText.text = "";
        }

        nameText.text = item.data.itemName;
        descriptionText.text = item.data.description;
        countText.text = $"Quantity: {item.count}";
    }

}