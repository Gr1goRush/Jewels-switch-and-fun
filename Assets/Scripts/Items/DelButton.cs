using UnityEngine;
using UnityEngine.UI;

public class DelButton : MonoBehaviour
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
        itemsController.DeleteItem(itemName);
    }
}
