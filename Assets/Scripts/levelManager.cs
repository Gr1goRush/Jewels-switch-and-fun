using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class levelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private Button GameButton;
    [SerializeField] private Image BackImage;
    [SerializeField] private List<Sprite> Sprites;

    void Start()
    {
        string activeLevel = PlayerPrefs.GetString("CurrentLevel");

        switch (activeLevel)
        {
            case "L 1":
                LevelText.text = "Level 1";
                BackImage.sprite = Sprites[0];
                break;
            case "L 2":
                LevelText.text = "Level 2";
                BackImage.sprite = Sprites[1];
                break;
            case "L 3":
                LevelText.text = "Level 3";
                BackImage.sprite = Sprites[2];
                break;
            case "L 4":
                LevelText.text = "Level 4";
                BackImage.sprite = Sprites[3];
                break;
            case "L 5":
                LevelText.text = "Level 5";
                BackImage.sprite = Sprites[4];
                break;
            case "L 6":
                LevelText.text = "Level 6";
                BackImage.sprite = Sprites[5];
                break;
            case "_completed":
                LevelText.text = "You have won each Level!";
                BackImage.sprite = Sprites[0];
                break;
            default:
                LevelText.text = "Start Game!";
                BackImage.sprite = Sprites[0];
                GameButton.interactable = true;
                break;
        }

        GameButton.onClick.AddListener(OnGameButtonClick);
    }

    private void OnGameButtonClick()
    {
        string activeLevel = PlayerPrefs.GetString("CurrentLevel");

        switch (activeLevel)
        {
            case "L 1":
                SetLevel("L 1");
                break;
            case "L 2":
                SetLevel("L 2");
                break;
            case "L 3":
                SetLevel("L 3");
                break;
            case "L 4":
                SetLevel("L 4");
                break;
            case "L 5":
                SetLevel("L 5");
                break;
            case "L 6":
                SetLevel("L 6");
                break;
            case "_completed":
                SetLevel("L 1");
                break;
            default:
                SetLevel("L 1");
                break;
        }
    }

    private void SetLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
