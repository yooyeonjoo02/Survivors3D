using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float moveSpeedBonus = 0f;
    [SerializeField] private int attackPower = 1;

    public int AttackPower => attackPower;

    public void IncreaseAttack()
    {
        attackPower += 1;
    }

    public void IncreaseMoveSpeed()
    {
        moveSpeedBonus += 1f;

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.AddMoveSpeed(1f);
    }
}