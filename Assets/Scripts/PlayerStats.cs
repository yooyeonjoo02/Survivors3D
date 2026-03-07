using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float moveSpeedBonus = 0f;
    [SerializeField] private int attackPower = 1;

    private const int ATTACK_INCREASE = 1;
    private const float SPEED_INCREASE = 1f;

    public int AttackPower => attackPower;

    public void IncreaseAttack()
    {
        attackPower += ATTACK_INCREASE;
    }

    public void IncreaseMoveSpeed()
    {
        moveSpeedBonus += SPEED_INCREASE;

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.AddMoveSpeed(1f);
    }
}