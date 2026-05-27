using System;
using System.Collections;
using UnityEngine;

public enum WaveState
{
    Idle,        // 시작 전
    Preparing,   // 웨이브 시작 직전 대기 중 (waveDelay)
    Spawning,    // 몬스터들을 풀어놓는 중 (spawnDuration 간격으로)
    InProgress,  // 다 풀어놨고 살아있는 몬스터 처치 대기 중
    Cleared,     // 한 웨이브 클리어 (다음 웨이브로 넘어가기 전 짧은 텀)
    AllCleared   // 모든 웨이브 끝 → 보스 트리거
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] WaveData[] waves;
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] SpawnPoint[] spawnPoints;
    [SerializeField] private GameObject Panelroot;
    [SerializeField] private HealthSystem playerHealth;

    public int CurrentWaveIndex { get; private set; } = -1;
    public int AliveCount { get; private set; }
    public WaveState State { get; private set; } = WaveState.Idle;
    public int TotalWaves => waves?.Length ?? 0;

    public event Action<int> OnWaveStarted;
    public event Action<int> OnWaveCleared;
    public event Action<int> OnAliveCountChanged;
    public event Action OnAllWavesCleared;

    private void OnEnable()
    {
        playerHealth.OnDeath += HandlePlayerDeath;
        DeadEndBehaviour.OnDeadAnimFinished += ShowRestartUI;
    }

    private void OnDisable()
    {
        playerHealth.OnDeath -= HandlePlayerDeath;
        DeadEndBehaviour.OnDeadAnimFinished -= ShowRestartUI;
    }

    private void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        StartCoroutine(RunWaves());
    }

    private void SpawnOne(MonsterData data, Transform at)
    {
        GameObject prefab = data.prefabOverride != null ? data.prefabOverride : monsterPrefab;
        var monster = Instantiate(prefab, at.position, at.rotation);
        monster.GetComponent<MonsterController>().SetData(data);
        var health = monster.GetComponent<MonsterHealth>();
        AliveCount++;
        OnAliveCountChanged?.Invoke(AliveCount);

        System.Action onDeath = null;
        onDeath = () =>
        {
            health.OnDeath -= onDeath;
            AliveCount--;
            OnAliveCountChanged?.Invoke(AliveCount);
        };
        health.OnDeath += onDeath;
    }

    private void HandlePlayerDeath()
    {
        StopAllCoroutines();
        Cursor.lockState = CursorLockMode.None;
        foreach (var m in FindObjectsByType<MonsterController>(FindObjectsSortMode.None))
            m.ChangeState(new MonsterIdleState());
    }

    private void ShowRestartUI()
    {
        Cursor.visible = true;
        Panelroot.SetActive(true);
    }

    private IEnumerator RunSingleWave(WaveData wave)
    {
        State = WaveState.Preparing;
        yield return new WaitForSeconds(wave.waveDelay);

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points!");
            yield break;
        }

        State = WaveState.Spawning;
        OnWaveStarted?.Invoke(CurrentWaveIndex);

        int spIdx = 0;
        foreach (var entry in wave.spawnEntries)
        {
            for (int n = 0; n < entry.count; n++)
            {
                SpawnOne(entry.monster, spawnPoints[spIdx % spawnPoints.Length].transform);
                spIdx++;
                if (wave.spawnDuration > 0)
                {
                    yield return new WaitForSeconds(wave.spawnDuration);
                }
            }
        }

        State = WaveState.InProgress;
        while (AliveCount > 0)
        {
            yield return null;
        }

        State = WaveState.Cleared;
        OnWaveCleared?.Invoke(CurrentWaveIndex);
    }

    private IEnumerator RunWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            CurrentWaveIndex = i;
            yield return RunSingleWave(waves[i]);
        }

        State = WaveState.AllCleared;
        OnAllWavesCleared?.Invoke();
    }

    [ContextMenu("DEBUG: Kill All Monsters")]
    private void DebugKillAll()
    {
        var healths = FindObjectsByType<MonsterHealth>(FindObjectsSortMode.None);
        foreach (var h in healths)
        {
            h.TakeDamage(9999);
        }
    }
}
