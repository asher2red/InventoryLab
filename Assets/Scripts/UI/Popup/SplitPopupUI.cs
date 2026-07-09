using TMPro;
using UnityEngine; 
using UnityEngine.UI;

public class SplitPopupUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField amountInput;

    [SerializeField]
    private TMP_Text maxAmountText;

    [SerializeField]
    private TMP_Text currentAmountText;

    [SerializeField]
    private Button confirmButton;

    private int maxSplitAmount;

    private System.Action<int> onConfirm;

    public void Open(int maxAmount, System.Action<int> callback)
    {
        gameObject.SetActive(true);

        maxSplitAmount = maxAmount;

        currentAmountText.text = $"Current : {maxAmount + 1}";
        maxAmountText.text = $"Max : {maxAmount}";

        amountInput.text = "";

        confirmButton.interactable = false;

        onConfirm = callback;

    }

    public void Confirm()
    {
        if (!int.TryParse(amountInput.text, out int amount))
        {
            return;
        }

        onConfirm?.Invoke(amount);

        Close();
    }

    public void Cancel()
    {
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ValidateInput(string value)
    {
        if (!int.TryParse(value, out int amount)) return;

        bool isValid = amount > 0 && amount <= maxSplitAmount;

        confirmButton.interactable = isValid;
    }

}