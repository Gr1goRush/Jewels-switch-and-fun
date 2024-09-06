using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    public static MoneyController Instance;

    [SerializeField] private List<TMP_Text> Money;
    [SerializeField] private GameObject Notification;
    [SerializeField] private List<Button> Buttons;

    private void Awake()
    {
        Instance = this;
        UpdateMoneyValue();
    }

    private void Start()
    {
        UpdateMoneyValue();
    }

    private void UpdateMoneyValue()
    {
        int moneyValue = PlayerPrefs.GetInt("_money", 0);
        if (moneyValue < 0)
        {
            PlayerPrefs.SetInt("_money", 0);
            moneyValue = 0;
        }

        foreach (var textElement in Money)
        {
            textElement.text = moneyValue.ToString();
        }

        bool hasMoney = moneyValue > 0;
        Notification.SetActive(!hasMoney);

        foreach (var button in Buttons)
        {
            button.interactable = hasMoney;
        }
    }

    public void CheckMoneyValue()
    {
        Debug.Log(PlayerPrefs.GetInt("_money"));
        if (PlayerPrefs.GetInt("_money") < 50)
        {
            Buttons[0].interactable = false;
            Buttons[1].interactable = false;
            Buttons[2].interactable = false;
        }
        else
        {
            Buttons[0].interactable = true;

            if (PlayerPrefs.GetInt("_money") < 100)
            {
                Buttons[1].interactable = false;
                Buttons[2].interactable = false;
            }
            else
            {
                Buttons[1].interactable = true;
                if (PlayerPrefs.GetInt("_money") < 250)
                {
                    Buttons[2].interactable = false;
                }
                else
                {
                    Buttons[0].interactable = true;
                    Buttons[1].interactable = true;
                    Buttons[2].interactable = true;
                }
            }
        }
    }

    public void AddMoney(int value)
    {
        int currentMoney = PlayerPrefs.GetInt("_money", 0);
        PlayerPrefs.SetInt("_money", Mathf.Max(0, currentMoney + value));
        UpdateMoneyValue();
    }

    public void RemoveMoney(int value)
    {
        int currentMoney = PlayerPrefs.GetInt("_money", 0);
        PlayerPrefs.SetInt("_money", Mathf.Max(0, currentMoney - value));
        UpdateMoneyValue();
    }
}