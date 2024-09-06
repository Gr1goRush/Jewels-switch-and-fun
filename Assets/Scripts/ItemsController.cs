using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemsController : MonoBehaviour
{
    [SerializeField] private TMP_Text BombCount;
    [SerializeField] private TMP_Text MagicWandCount;
    [SerializeField] private TMP_Text FireworkCount;

    [SerializeField] private Button bombButton;
    [SerializeField] private Button magicWandButton;
    [SerializeField] private Button fireworkButton;

    private const string BombKey = "BombCount";
    private const string MagicWandKey = "MagicWandCount";
    private const string FireworkKey = "FireworkCount";

    private int bombCount;
    private int magicWandCount;
    private int fireworkCount;

    private void Awake() => LoadItems();

    private void UpdateUI()
    {
        BombCount.text = $"X{bombCount}";
        MagicWandCount.text = $"X{magicWandCount}";
        FireworkCount.text = $"X{fireworkCount}";

        CheckValues();

        if (SceneManager.GetActiveScene().name == "_menu") MoneyController.Instance.CheckMoneyValue();
        else return;
    }

    private void CheckValues()
    {
        if (bombCount == 0)
        {
            bombButton.interactable = false;
        }
        if (magicWandCount == 0)
        {
            magicWandButton.interactable = false;
        }
        if (fireworkCount == 0)
        {
            fireworkButton.interactable = false;
        }
    }

    private void LoadItems()
    {
        bombCount = PlayerPrefs.GetInt(BombKey, 0);
        magicWandCount = PlayerPrefs.GetInt(MagicWandKey, 0);
        fireworkCount = PlayerPrefs.GetInt(FireworkKey, 0);

        CheckValues();
        UpdateUI();
    }

    private void SaveItems()
    {
        PlayerPrefs.SetInt(BombKey, bombCount);
        PlayerPrefs.SetInt(MagicWandKey, magicWandCount);
        PlayerPrefs.SetInt(FireworkKey, fireworkCount);
        PlayerPrefs.Save();
    }

    public void BuyItem(string itemName)
    {
        switch (itemName)
        {
            case "Bomb":
                bombCount++;
                SaveItems();
                UpdateUI();
                break;
            case "MagicWand":
                magicWandCount++;
                SaveItems();
                UpdateUI();
                break;
            case "Firework":
                fireworkCount++;
                SaveItems();
                UpdateUI();
                break;
            default:
                Debug.LogWarning("Unknown item name: " + itemName);
                break;
        }
    }

    public void DeleteItem(string itemName)
    {
        switch (itemName)
        {
            case "Bomb":
                if (bombCount > 0)
                {
                    bombCount--;
                    SaveItems();
                    UpdateUI();
                }
                else
                {
                    Debug.LogWarning("Cannot delete Bomb: count is already 0");
                }
                break;
            case "MagicWand":
                if (magicWandCount > 0)
                {
                    magicWandCount--;
                    SaveItems();
                    UpdateUI();
                }
                else if (magicWandCount == 0)
                {
                    magicWandButton.interactable = false;
                }
                else
                {
                    Debug.LogWarning("Cannot delete Magic Wand: count is already 0");
                }
                break;
            case "Firework":
                if (fireworkCount > 0)
                {
                    fireworkCount--;
                    SaveItems();
                    UpdateUI();
                }
                else if (fireworkCount == 0)
                {
                    fireworkButton.interactable = false;
                }
                else
                {
                    Debug.LogWarning("Cannot delete Firework: count is already 0");
                }
                break;
            default:
                Debug.LogWarning("Unknown item name: " + itemName);
                break;
        }
    }
}