# Wave Rush

액션 핵 앤 슬래시 — 5웨이브 + 보스전
1인 제작 / 개발 기간 3주 / Unity 6 · URP

---

## 실행 안내 (먼저 읽어주세요)

**이 저장소는 유료 에셋을 포함하지 않습니다.**

`Assets/Imported/` 는 학원에서 라이선스를 제공받은 유료 에셋이라 저작권상 커밋에서 제외했습니다. 따라서 클론만으로는 프로젝트가 정상 실행되지 않으며, **코드와 설계 확인용 저장소**입니다.

- 플레이 영상: `(링크 추가 예정)`
- 실행 빌드: `(링크 추가 예정)`

Firebase 기능을 사용하려면 `Assets/google-services.json` 이 별도로 필요합니다 (보안상 제외).

---

## 기술 스택

| 항목 | 버전 |
|---|---|
| Unity | 6000.3.15f1 |
| URP | 17.3.0 |
| Input System | 1.19.0 |
| Cinemachine | 3.1.6 |
| UniTask | Cysharp (git) |
| Firebase | Auth · Realtime Database |

C# 스크립트 87개.

---

## 아키텍처

같은 프로젝트 안에서 **FSM 두 가지와 BT 하나**를 목적에 따라 다르게 구현했습니다.

### 플레이어 — 애니메이션이 상태의 원천

`CharacterStateMachine` 은 전이 로직이 없는 순수 상태 홀더입니다. 실제 전이는 각 애니메이션 State에 붙은 `PlayerStateBehaviour`(StateMachineBehaviour)가 `OnStateEnter` 에서 밀어넣습니다.

```
Animator State ──PlayerStateBehaviour──▶ CurrentState ──▶ 입력·이동 게이팅
```

코드는 상태를 만들지 않고 읽기만 하므로 애니메이션과 로직이 어긋날 수 없습니다. 대신 흐름이 Animator 에디터에 분산되는 트레이드오프가 있습니다.

- `CharacterMover` — `Idle`/`Moving` 일 때만 이동 허용
- `PlayerCombat` — `Dodging`/`Damaged`/`Dead` 면 입력 차단
- `InvincibilityBehaviour` — 회피 구간에 `IsInvincible` 토글 (i-frame)

### 콤보 버퍼 — 입력 큐를 만들지 않은 이유

콤보 타이밍의 원천이 애니메이션 클립이므로, 별도 입력 큐를 두면 타이밍 소스가 이중화되어 동기화 버그가 생깁니다. 그래서 **Animator 트리거가 트랜지션에 소비될 때까지 유지되는 성질**을 1-depth 버퍼로 활용했습니다.

| | 버퍼 정책 | 캔슬 |
|---|---|---|
| 약공 (LMB) | 트리거 잔존 — 관대 | 회피로 캔슬 가능 |
| 강공 (RMB) | 애니메이션 이벤트 윈도우 — 엄격 | 캔슬 불가 |

부작용도 있었습니다. 소비되지 않은 트리거가 남아 회피 직후 공격이 튀어나오는 문제가 있어, `SetTriggerExclusive` 로 다른 액션 트리거를 배타적으로 리셋하도록 했습니다.

회피만은 `anim.Play(hash, 0, 0f)` 로 즉시 재생합니다. 생존기는 반응성이 우선이라 버퍼링하지 않습니다. AGI 스탯이 애니메이션 속도를 올리지만 회피 중에는 `anim.speed = 1` 로 고정해, i-frame 길이가 스탯에 따라 변하지 않게 했습니다.

### 몬스터 — 데이터가 타입을 결정

플레이어와 **반대 방향**입니다. 코드가 상태를 정하고 애니메이션을 트리거합니다. 몬스터는 거리·쿨다운 판단이 상태의 본체이기 때문입니다.

```csharp
public interface IMonsterState { void Enter(ctx); void Tick(ctx); void Exit(ctx); }
```

3종 몬스터는 별개 클래스가 아니라 컨트롤러 하나 + `MonsterData`(SO) 값 분기입니다.

```csharp
public IMonsterState CreateAttackState()
{
    if (Data.projectilePrefab != null) return new MonsterRangedAttackState();
    if (Data.telegraphTime > 0f)       return new MonsterTelegraphAttackState();
    return new MonsterAttackState();
}
```

새 몬스터는 코드가 아니라 데이터로 추가됩니다.

`Exit()` 을 쓴 실질적 이유도 있습니다. 엘리트의 예고 동작에서 빠져나가는 경로가 셋(거리 벌어짐 / 피격 / 사망)인데 둘은 이벤트로 들어옵니다. 정리 코드를 `Exit()` 에 두면 호출자가 무엇을 하든 예고 모션이 남지 않습니다.

상태 개수를 늘리지 않으려 한 판단도 일관됩니다 — 원거리의 후퇴(Kiting)는 별도 상태가 아니라 `MonsterChaseState` 내부 행동이고, 엘리트의 2단계 공격도 `fired` 플래그로 한 상태 안에서 처리합니다.

### 보스 — Custom Behavior Tree

`BTNode` / `BTSelector` / `BTSequence` 를 직접 구현했습니다.

BT는 매 프레임 Root부터 재평가하므로 "지금 무엇을 하는 중"이라는 개념이 없습니다. 그냥 두면 돌진 중에 다음 프레임이 다른 패턴으로 갈아탑니다. 그래서 진행 중 동작을 Blackboard 플래그로 기억하고 **Continue 노드를 트리 맨 앞에** 두었습니다.

```
Root · Selector
├─ ContinueChargeAction      isCharging  → Running (아래 차단)
├─ ContinueTelegraphAction   isTelegraphing → Running
├─ Sequence                  CheckDistance → DecidePattern → 공격 실행
└─ ChaseAction
```

패턴은 거리 구간별 확률로 선택하고, 쿨다운 중인 패턴이 뽑히면 근접으로 대체해 연속 사용을 막습니다. 돌진 방향은 발동 순간 고정이라 플레이어가 옆으로 회피할 수 있습니다.

### 웨이브 — ScriptableObject 조립

```
WaveData(SO) → spawnEntries[MonsterData(SO) × count] → 공격 상태 자동 결정
             └ bossPrefabOverride → 보스 웨이브
```

보스를 위한 특수 코드가 없습니다. 마지막 웨이브 SO의 필드 하나를 채우면 보스 웨이브가 됩니다. 웨이브 전환은 이벤트로 전파합니다 (`OnWaveCleared` → 상점 → 다음 웨이브, `OnAllWavesCleared` → 보스).

### UI — PauseManager 레퍼런스 카운팅

웨이브 클리어 후 상점이 열리고 그 위에 레벨업 스탯창이 겹치는 상황이 있습니다. `bool` 하나로 관리하면 상점만 닫아도 게임이 재개되어버립니다.

```csharp
public void Pause()  { pauseCount++; Time.timeScale = 0f; ApplyCursor(); }
public void Resume() { pauseCount = Mathf.Max(0, pauseCount - 1);
                       if (pauseCount == 0) Time.timeScale = 1f; ApplyCursor(); }
```

마지막 하나가 닫힐 때만 재개하며, 커서 표시/잠금도 같은 기준을 따릅니다.

---

## 성장 · 경제

성장 경로가 둘인데 성격이 다릅니다.

```
몬스터 처치 ─┬─ EXP  → 레벨업 → 포인트 +2 → 원하는 스탯에 분배   (선택)
             └─ Gold → 상점  → 랜덤 스탯 4g → 무작위 스탯 +1     (운)
```

후반에 골드가 남는 문제가 있어, 가장 싼 영구 강화(랜덤 스탯 4g)를 무한 골드 싱크로 두었습니다. 다만 어떤 스탯이 오를지는 운이라는 점이 남은 트레이드오프입니다. 확정 강화로 바꾸면 빌드가 하나로 수렴하므로, 개선한다면 랜덤 3개 중 택1 방식이 적절하다고 봅니다.

요구 경험치는 `50 + (Lv-1) × 10` 선형이며, 상점은 포션 +1(15g) · HP 회복(10g) · 랜덤 스탯(4g) 3종입니다.

---

## 프로젝트 구조

```
Assets/Script/
├─ Character/
│  ├─ CharacterMove/    입력 · FSM · 전투 · 이동
│  ├─ CharacterStat/    스탯 · 체력 · 스태미나 · 경험치
│  └─ Monster/          몬스터 FSM · 투사체
│     └─ Boss/          BT 노드 · Blackboard
├─ Wave/                웨이브 관리 · 상점
├─ UI/                  HUD · 패널 · PauseManager
├─ Firebase/            인증 · 리더보드 · 프로필
└─ Common/              오디오 · 토스트 · 데미지 팝업
```

---

## 조작

| 입력 | 동작 |
|---|---|
| `LMB` | 약공 3타 콤보 |
| `RMB` | 강공 콤보 (스태미나 소비) |
| `Space` | 회피 — 입력 방향 · i-frame |
| `E` | 포션 |
| `ESC` | 일시정지 |
