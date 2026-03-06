using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 10;
    [SerializeField] private Slider hpBar;

    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;

        if (hpBar != null)
        {
            hpBar.minValue = 0;
            hpBar.maxValue = maxHP;
            hpBar.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        if (hpBar != null)
            hpBar.value = currentHP;

        if (currentHP <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}
