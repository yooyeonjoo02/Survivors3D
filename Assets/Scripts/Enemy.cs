using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("HP")]
    [SerializeField] private int maxHp = 1;
    private int currentHp;

    [Header("FX")]
    [SerializeField] private GameObject deathFxPrefab;

    [Header("Drop")]
    [SerializeField] private GameObject gemPrefab;

    private const int MIN_HP = 1;
    private const float FLAT_Y_DIRECTION = 0f;
    private const float ROTATION_THRESHOLD = 0.0001f;
    private const int PLAYER_COLLISION_DAMAGE = 1;
    private const int SCORE_PER_KILL = 1;

    private Transform target;
    private bool isDead = false;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void ApplyEliteBuff(float hpMultiplier)
    {
        int newMax = Mathf.CeilToInt(maxHp * hpMultiplier);
        maxHp = Mathf.Max(MIN_HP, newMax);
        currentHp = maxHp;
    }

    public void AddMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if (target == null || isDead) return;

        Vector3 dir = target.position - transform.position;
        dir.y = FLAT_Y_DIRECTION;
        dir = dir.normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

        if (dir.sqrMagnitude > ROTATION_THRESHOLD)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(PLAYER_COLLISION_DAMAGE);
            }

            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(SCORE_PER_KILL);
        }

        if (gemPrefab != null)
        {
            Instantiate(gemPrefab, transform.position, Quaternion.identity);
        }

        if (deathFxPrefab != null)
        {
            Instantiate(deathFxPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}