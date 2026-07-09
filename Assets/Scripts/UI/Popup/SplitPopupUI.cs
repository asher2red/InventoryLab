using TMPro;
using UnityEngine; 
using UnityEngine.UI;

public class SplitPopupUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField amountInput;

    private System.Action<int> onConfirm;

    public void Open(System.Action<int> callback)
    {
        gameObject.SetActive(true);

        amountInput.text = "";

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

}