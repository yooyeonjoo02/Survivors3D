using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveUI waveUI;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnDistance = 12f;

    [Header("Difficulty Scaling")]
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float intervalDecreaseRate = 0.9f; // 30초마다 10% 감소

    [Header("Elite")]
    [SerializeField] private float eliteHpMultiplier = 3f;
    [SerializeField] private float eliteScaleMultiplier = 2f;

    private float timer;

    private int lastStage = -1; 
    private int lastWave = -1;  

    private void Update()
    {
        if (enemyPrefab == null || player == null) return;

        HandleDifficultyScaling();
        HandleWaveAndElite();

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    private void HandleDifficultyScaling()
    {
        int stage = Mathf.FloorToInt(Time.time / 30f);

        if (stage != lastStage)
        {
            lastStage = stage;

            spawnInterval *= intervalDecreaseRate;
            spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);

            Debug.Log($"[Stage {stage}] spawnInterval = {spawnInterval}");
        }
    }

    private void HandleWaveAndElite()
    {
        int wave = Mathf.FloorToInt(Time.time / 5f);

        if (wave != lastWave)
        {
            lastWave = wave;

            int waveNumber = wave + 1;

            Debug.Log($"Wave {waveNumber} Start!");

            if (waveUI != null)
            {
                waveUI.ShowWaveText(waveNumber);
            }

            SpawnElite();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = GetSpawnPosition();
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // Enemy 스크립트에 타겟 전달
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetTarget(player);
        }
    }

    private void SpawnElite()
    {
        Vector3 spawnPos = GetSpawnPosition();
        GameObject eliteObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        eliteObj.transform.localScale *= eliteScaleMultiplier;

        Enemy enemy = eliteObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetTarget(player);

            enemy.ApplyEliteBuff(eliteHpMultiplier);
            Debug.Log("ELITE BUFF APPLIED!");
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector2 rand = Random.insideUnitCircle.normalized * spawnDistance;
        return new Vector3(player.position.x + rand.x, 1f, player.position.z + rand.y);
    }
}