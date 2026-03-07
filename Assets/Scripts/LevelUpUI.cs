using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private Button attackUpButton;
    [SerializeField] private Button speedUpButton;

    private PlayerStats playerStats;

    private const float GAME_PAUSE_TIME_SCALE = 0f;
    private const float GAME_NORMAL_TIME_SCALE = 1f;

    private void Start()
    {
        levelUpPanel.SetActive(false); // 시작 시 창 숨기기

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        attackUpButton.onClick.AddListener(OnClickAttackUp);
        speedUpButton.onClick.AddListener(OnClickSpeedUp);
    }

    private void Open()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = GAME_PAUSE_TIME_SCALE;
    }

    private void Close()
    {
        Time.timeScale = GAME_NORMAL_TIME_SCALE;
        levelUpPanel.SetActive(false);
    }

    private void OnClickAttackUp()
    {
        playerStats.IncreaseAttack();
        Close();
    }

    private void OnClickSpeedUp()
    {
        playerStats.IncreaseMoveSpeed();
        Close();
    }
}