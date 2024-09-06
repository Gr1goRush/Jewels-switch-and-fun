using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public ItemsController itemsController;
    public string itemName;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        itemsController.BuyItem(itemName);
    }
}
