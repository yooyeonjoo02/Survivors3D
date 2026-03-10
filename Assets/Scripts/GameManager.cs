using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string BEST_TIME_KEY = "BestSurvivalTime";
    private const string SCORE_TEXT_PREFIX = "Score : ";
    private const string CURRENT_TIME_TEXT_PREFIX = "Time : ";
    private const string BEST_TIME_TEXT_PREFIX = "Best Time : ";

    private const int INITIAL_SCORE = 0;
    private const float INITIAL_SURVIVAL_TIME = 0f;
    private const float GAME_RUNNING_TIME_SCALE = 1f;
    private const float GAME_STOP_TIME_SCALE = 0f;

    private const float SECONDS_PER_MINUTE = 60f;
    private const int DEFAULT_BEST_TIME = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private int score = INITIAL_SCORE;
    private bool isGameOver = false;
    private float survivalTime = INITIAL_SURVIVAL_TIME;

    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = GAME_RUNNING_TIME_SCALE;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateScoreUI();
    }

    private void Update()
    {
        if (isGameOver) return;

        survivalTime += Time.deltaTime;
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{SCORE_TEXT_PREFIX}{score}";
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, DEFAULT_BEST_TIME);

        if (survivalTime > bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, survivalTime);
            PlayerPrefs.Save();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        UpdateGameOverUI();

        Time.timeScale = GAME_STOP_TIME_SCALE;
    }

    private void UpdateGameOverUI()
    {
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, DEFAULT_BEST_TIME);

        if (currentTimeText != null)
        {
            currentTimeText.text = $"{CURRENT_TIME_TEXT_PREFIX}{FormatTime(survivalTime)}";
        }

        if (bestTimeText != null)
        {
            bestTimeText.text = $"{BEST_TIME_TEXT_PREFIX}{FormatTime(bestTime)}";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / SECONDS_PER_MINUTE);
        int seconds = Mathf.FloorToInt(time % SECONDS_PER_MINUTE);

        return $"{minutes:00}:{seconds:00}";
    }

    public void Restart()
    {
        Time.timeScale = GAME_RUNNING_TIME_SCALE;

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}