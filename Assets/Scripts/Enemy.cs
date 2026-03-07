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
        maxHp = Mathf.Max(1, newMax);
        currentHp = maxHp;
    }

    public void AddMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void TakeDamagePublic(int damage)
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

        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;
        dir = dir.normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

        if (dir.sqrMagnitude > 0.0001f)
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
                ph.TakeDamage(1);

            Die();
        }
    }

    /*
    private void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }
    */

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(1); // 점수 +1
        }

        if (gemPrefab != null)
        {
            Instantiate(gemPrefab, transform.position, Quaternion.identity); // Gem
        }

        if (deathFxPrefab != null)
        {
            Instantiate(deathFxPrefab, transform.position, Quaternion.identity); // 이펙트 
        }

        Destroy(gameObject);
    }
}