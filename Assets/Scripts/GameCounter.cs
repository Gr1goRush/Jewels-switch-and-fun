using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameCounter : MonoBehaviour
{
    public static GameCounter Instance { get; private set; }

    private int _step;
    private int _score;

    [SerializeField] private TextMeshProUGUI stepText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TMP_Text targetText;

    [SerializeField] private int maxSteps;
    [SerializeField] private int maxScore;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [SerializeField] private GameObject gameScene;

    private void Awake() => Instance = this;

    private void Start()
    {
        targetText.text = maxScore.ToString();
        scoreText.text = "0";
        stepText.text = maxSteps.ToString();

        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
    }

    public int Step
    {
        get => _step;
        set
        {
            if (_step == value) return;

            _step = value;
            stepText.SetText($"{maxSteps - _step}");

            CheckGameConditions();
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            if (_score == value) return;

            _score = value;
            scoreText.SetText($"{_score}");

            CheckGameConditions();
        }
    }

    private void CheckGameConditions()
    {
        //if (_score == maxScore && maxSteps - _step == 0) WinLevel();
        //else if (_score == maxScore) WinLevel();
        //else if (maxSteps - _step == 0) LoseLevel();
        //{
        //    if (_score == maxScore && maxSteps - _step == 0) WinLevel();
        //    else return;
        //}

        if (_score == maxScore || maxSteps - _step == 0)
        {
            if (_score == maxScore) WinLevel();
            else if (maxSteps - _step == 0 && _score == maxScore)
            {
                AudioManager.Instance.sfxSource.Stop();
                loseScene.SetActive(false);
                winScene.SetActive(true);
                return;
            }
            else if (maxSteps - _step == 0) LoseLevel();
            else return;
        }
        else return;
    }

    private void WinLevel()
    {
        loseScene.SetActive(false);
        winScene.SetActive(true);

        if (MoneyController.Instance != null)
        {
            MoneyController.Instance.AddMoney(250);
        }
        else
        {
            Debug.LogError("MoneyController instance is null!");
            PlayerPrefs.SetInt("_money", (PlayerPrefs.GetInt("_money", 0) + 250));
        }
        AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.PlaySFX("Win");

        if (gameScene != null) gameScene.SetActive(false);
        else return;
    }

    private void LoseLevel()
    {
        AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.PlaySFX("Lose");

        if (gameScene != null) gameScene.SetActive(false);
        winScene.SetActive(false);
        loseScene.SetActive(true);
    }
}
