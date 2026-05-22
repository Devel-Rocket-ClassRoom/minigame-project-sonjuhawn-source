# 진행 상황

## 완료

### #13 몬스터 웨이브
- WaveManager / WaveData / WaveHud / SpawnPoint 구현 완료
- WaveData: SpawnEntry(monster, count), waveDelay, spawnDuration
- WaveManager: Idle→Preparing→Spawning→InProgress→Cleared→AllCleared FSM
- WaveHud: OnWaveStarted / OnAliveCountChanged / OnAllWavesCleared 구독

### #29 캐릭터 Dead + 재시작 UI
- PlayerCombat: HealthSystem 캐싱, OnDeath 구독, HandleDeath (Die 트리거 + ChangeState(Dead))
- WaveManager: HandlePlayerDeath (StopAllCoroutines + 몬스터 전체 Idle)
- DeadEndBehaviour: Dead 애니메이션 스테이트에 부착, OnStateExit → OnDeadAnimFinished 이벤트
- WaveManager: ShowRestartUI (Panelroot SetActive)
- RestartPanel: 다시 시작(SceneManager.LoadScene) / 종료(Application.Quit) 버튼
- StartPanel: WaveManager.StartGame() — 시작 버튼 누르면 RunWaves 시작

### 버그 수정
- Dodge 제자리 문제: Inspector에서 dodgeDistance=0.1, dodgeDuration=2 잘못 설정되어 있었음 → 수정
- DodgeMove 코루틴: yield return null → WaitForFixedUpdate, Time.deltaTime → Time.fixedDeltaTime

---

## TODO (다음 세션)
- 자잘한 이슈 목록 정리 후 수정
- 빌드 준비 (debug input 제거 등)
