using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }

    private int _score;

    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return;
            _score = value;

            scoreText.SetText($"{_score}");
            maxScoreText.SetText($"{maxScore}");
            WinManager();
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI maxScoreText;
    [SerializeField] private string sceneName;
    [SerializeField] private int maxScore;
    [SerializeField] private GameObject winScene;

    private static GameObject gameScene;

    private void Awake()
    {
        Instance = this;
        if (gameScene == null)
            gameScene = GameObject.Find("_main");
    }

    private void Start()
    {
        if (maxScore == 0)  maxScore = PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}maxScore");
        else PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}maxScore", maxScore);

        scoreText.SetText($"Score: 0/{maxScore}");
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
    }

    private void WinManager()
    {
        if (_score == maxScore)
        {
            if (MoneyController.Instance != null)
            {
                MoneyController.Instance.AddMoney(250);
            }
            else
            {
                Debug.LogError("MoneyController instance is null!");
                PlayerPrefs.SetInt("_money", (PlayerPrefs.GetInt("_money", 0) + 250));
            }
            AudioManager.Instance.PlaySFX("Win");

            if (gameScene != null) gameScene.SetActive(false);
            else Debug.LogError("gameScene is not assigned!");

            winScene.SetActive(true);
        }
    }
}
