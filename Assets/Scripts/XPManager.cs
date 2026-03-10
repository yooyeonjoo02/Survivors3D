using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    private const int INITIAL_LEVEL = 1;
    private const int INITIAL_XP = 0;
    private const int INITIAL_MAX_XP = 5;
    private const int XP_INCREASE_PER_LEVEL = 3;

    [Header("XP / Level")]
    [SerializeField] private int level = INITIAL_LEVEL;
    [SerializeField] private int currentXP = INITIAL_XP;
    [SerializeField] private int maxXP = INITIAL_MAX_XP;

    public int Level => level;
    public int CurrentXP => currentXP;
    public int MaxXP => maxXP;

    public System.Action OnLevelUp;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"현재 XP: {currentXP}/{maxXP}");

        if (currentXP >= maxXP)
        {
            currentXP -= maxXP;
            level++;
            Debug.Log("레벨업!");

            maxXP += XP_INCREASE_PER_LEVEL;

            OnLevelUp?.Invoke();
        }
    }
}