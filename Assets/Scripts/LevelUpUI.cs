using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private Button attackUpButton;
    [SerializeField] private Button speedUpButton;

    private PlayerStats playerStats;

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
        Time.timeScale = 0f;
    }

    private void Close()
    {
        Time.timeScale = 1f;
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