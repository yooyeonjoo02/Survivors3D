using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private const int MIN_HP = 0;

    [SerializeField] private int maxHP = 10;
    [SerializeField] private Slider hpBar;

    private int currentHP;
    private bool isDead = false;

    private void Start()
    {
        currentHP = maxHP;

        if (hpBar != null)
        {
            hpBar.minValue = MIN_HP;
            hpBar.maxValue = maxHP;
            hpBar.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, MIN_HP);

        if (hpBar != null)
            hpBar.value = currentHP;

        if (currentHP <= MIN_HP)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameManager.Instance.GameOver();
    }
}