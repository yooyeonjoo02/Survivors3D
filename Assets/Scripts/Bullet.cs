using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamagePublic(1); 
            }

            Destroy(gameObject); 
        }
    }
}
// 엄청 커다란 모기가 나의 발을 물었어