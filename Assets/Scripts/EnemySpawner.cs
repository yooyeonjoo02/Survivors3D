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
    [SerializeField] private float intervalDecreaseRate = 0.9f;

    [Header("Elite")]
    [SerializeField] private float eliteHpMultiplier = 3f;
    [SerializeField] private float eliteScaleMultiplier = 2f;

    private const int INITIAL_STAGE_INDEX = -1;
    private const int INITIAL_WAVE_INDEX = -1;
    private const float TIMER_RESET = 0f;
    private const float STAGE_DURATION = 30f;
    private const float WAVE_DURATION = 5f;
    private const int DISPLAY_WAVE_OFFSET = 1;
    private const float SPAWN_Y_POSITION = 1f;

    private float timer;
    private float elapsedTime;
    private float initialSpawnInterval;

    private int lastStage = INITIAL_STAGE_INDEX;
    private int lastWave = INITIAL_WAVE_INDEX;

    private void Start()
    {
        initialSpawnInterval = spawnInterval;

        timer = 0f;
        elapsedTime = 0f;
        spawnInterval = initialSpawnInterval;

        lastStage = INITIAL_STAGE_INDEX;
        lastWave = INITIAL_WAVE_INDEX;
    }

    private void Update()
    {
        if (enemyPrefab == null || player == null) return;

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        elapsedTime += Time.deltaTime;

        HandleDifficultyScaling();
        HandleWaveAndElite();

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = TIMER_RESET;
            SpawnEnemy();
        }
    }

    private void HandleDifficultyScaling()
    {
        int stage = Mathf.FloorToInt(elapsedTime / STAGE_DURATION);

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
        int wave = Mathf.FloorToInt(elapsedTime / WAVE_DURATION);

        if (wave != lastWave)
        {
            lastWave = wave;

            int waveNumber = wave + DISPLAY_WAVE_OFFSET;

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
        return new Vector3(player.position.x + rand.x, SPAWN_Y_POSITION, player.position.z + rand.y);
    }
}