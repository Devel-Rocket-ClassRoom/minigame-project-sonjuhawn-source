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

### #34 골드 시스템 + EXP/레벨 UI
- GoldSystem: AddGold / SpendGold / OnGoldChanged 이벤트
- MonsterController.HandleDeath: goldReward 지급 추가
- ExpBar: OnExpChanged 구독 → Slider, OnLevelUp 구독 → TMP_Text (Lv.)
- GoldHud: OnGoldChanged 구독 → TMP_Text

### #34 보스 BT
- BT 코어: BTNode / BTSelector / BTSequence
- BossBlackboard: 공유 데이터 (self, target, anim, 쿨다운, 차징 상태)
- BossData SO: 기본/AI범위/차징/보상 필드
- BossHealth: IDamageable 구현, OnDeath/OnDamaged 이벤트
- BossController: BT 조립 + Update Tick + 사망 보상 지급
- 노드 구성:
  - CheckDistance (within 플래그로 안/밖 양방향)
  - ChaseAction (이동 + 회전 + Move float)
  - DecidePatternAction (거리 기반 패턴 선택, 근접 범위 내 랜덤)
  - MeleeAttackAction (pattern==0, 애니 이벤트 OnAttackHit)
  - ChargeAction (pattern==1, 예비동작 시작)
  - ContinueTelegraphAction (예비동작 중 플레이어 추적, 완료 시 돌진 시작)
  - ContinueChargeAction (돌진 유지, 완료 시 공격 쿨다운 연동)
- 돌진 데미지: OnTriggerEnter (Is Trigger 콜라이더)

---

## TODO (추후)
- 보스 원거리 공격 패턴 추가
  - RangedAttackAction 노드
  - BossData에 projectilePrefab / rangedRange / rangedCooldown 필드
  - DecidePatternAction에 원거리 패턴(2) 추가
- 빌드 준비 (debug input 제거 등)
