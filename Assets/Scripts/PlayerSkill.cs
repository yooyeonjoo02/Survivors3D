using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerSkill : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] private float skillRadius = 5f;
    [SerializeField] private int skillDamage = 999;
    [SerializeField] private float cooldown = 10f;

    [Header("UI")]
    [SerializeField] private Slider cooldownSlider;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private float lastUseTime = -10f;

    private void Start()
    {
        UpdateCooldownUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanUseSkill())
        {
            UseSkill();
        }

        UpdateCooldownUI();
    }

    private bool CanUseSkill()
    {
        return Time.time >= lastUseTime + cooldown;
    }

    private void UseSkill()
    {
        lastUseTime = Time.time;

        Collider[] hits = Physics.OverlapSphere(transform.position, skillRadius);
        bool hitEnemy = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(skillDamage);
                    hitEnemy = true;
                }
            }
        }

        if (hitEnemy && impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }

    private void UpdateCooldownUI()
    {
        if (cooldownSlider == null) return;

        float elapsed = Time.time - lastUseTime;
        float ratio = Mathf.Clamp01(elapsed / cooldown);
        cooldownSlider.value = ratio;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
}