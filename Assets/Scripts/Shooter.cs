using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float bulletLifeTime = 3f;

    [Header("Sound")]
    [SerializeField] private AudioClip shootClip;

    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource 자동 확보
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;   // 2D 사운드
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (muzzle == null || bulletPrefab == null)
        {
            Debug.LogWarning("Shooter: muzzle 또는 bulletPrefab이 비어있음!");
            return;
        }

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        // 발사
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = muzzle.forward * bulletSpeed;
        }

        // 총소리
        if (shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }

        Destroy(bullet, bulletLifeTime);
    }
}
