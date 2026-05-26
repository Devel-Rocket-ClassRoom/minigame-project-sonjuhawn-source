# MinigameProject 포트폴리오

## 프로젝트 개요
- BDO 모티브 3D 액션 게임 (솔로 개발)
- Unity URP / InputSystem / 3주 개발

---

## 아키텍처 설계 포인트

### 인터페이스 기반 설계
- `IDamageable` — 플레이어/몬스터/보스가 동일 인터페이스 구현
  - SwordHitbox가 대상 타입 몰라도 데미지 처리 가능
- `IStatProvider` — UI/전투/스태미나가 PlayerStats 직접 참조 대신 인터페이스로 통신
  - 의존성 역전 원칙(DIP) 적용

### 이벤트 기반 UI (Observer 패턴)
- HpBar / ExpBar / GoldHud / PotionHud 모두 이벤트 구독 방식
- 폴링 없이 상태 변화 시에만 업데이트

### ScriptableObject 데이터 주입
- MonsterData / BossData로 데이터와 로직 분리
- 인스펙터에서 밸런싱 가능, 코드 수정 없이 몬스터 추가

### SwordHitbox 멀티히트 방지
- HashSet<IDamageable>으로 한 스윙에 동일 대상 중복 피격 구조적 차단

---

## 몬스터 AI

### 일반 몬스터 — FSM
- 상태: Idle / Chase / Attack / Damaged / Dead
- IMonsterState 인터페이스로 상태 분리
- MonsterData SO로 타입별 데이터 주입 (근접/원거리/엘리트)
- 개선 포인트: CreateAttackState() 분기가 Controller에 있음 → MonsterData로 이동하면 OCP 충족

### 보스 — BT (직접 구현)
- BTNode / BTSelector / BTSequence 직접 구현
- BossBlackboard로 노드 간 데이터 공유 (Observer 없이 단방향 데이터 흐름)
- 노드 구성:
  - CheckDistance — within 플래그로 안/밖 양방향 재사용
  - DecidePatternAction — 거리 기반 패턴 선택 + 쿨다운 체크
  - ContinueChargeAction — 최상단 배치로 돌진 중 다른 행동 차단
- 개선 포인트: ContinueCharge/Telegraph 중복 구조 → 추상 클래스로 통합 가능

---

## 플레이어 시스템
- FSM 기반 상태 관리 (Animator StateMachineBehaviour 연동)
- 스태미나 루프: 일반공격 회복 / 강공격·구르기 소모
- 콤보 버퍼 + 무적 프레임 (i-frame)
- 레벨업 시 스탯 분배 UI (STR/AGI/VIT/STA)

---

## 웨이브 시스템
- WaveData SO로 웨이브별 몬스터 구성 정의
- Idle → Preparing → Spawning → InProgress → Cleared → AllCleared
- 사망 콜백으로 생존 몬스터 추적