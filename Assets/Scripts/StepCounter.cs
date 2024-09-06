using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StepCounter : MonoBehaviour
{
    public static StepCounter Instance { get; private set; }

    private int _score;

    public int Step
    {
        get => _score;

        set
        {
            if (_score == value) return;
            _score = value;

            scoreText.SetText(sourceText: $"{maxScore - _score} moves left");
            WinManager(sceneName, loseScene, gameScene);
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string sceneName;
    [SerializeField] private int maxScore;
    [SerializeField] private GameObject loseScene;
    [SerializeField] private GameObject gameScene;

    private void Awake() => Instance = this;

    private void Start()
    {
        scoreText.SetText(sourceText: $"{maxScore} moves left");
    }

    private void WinManager(string sceneName, GameObject loseScene,  GameObject gameScene)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if ((maxScore - _score) == 0)
            {
                gameScene.SetActive(false);
                loseScene.SetActive(true);

                AudioManager.Instance.PlaySFX("Lose");
            }
        }

    }
}