# Project Summary (Ultra Compact)

## Genre
- BDO-style action hack & slash
- Wave-based (5–10 stages + boss)

---

## Core Combat
- LMB: 3-hit combo (stamina +)
- RMB: heavy combo (stamina -)
- Space: dodge (i-frame, stamina -, short CD)

### Design
- InputSystem
- combo buffer
- stamina loop (light gain / others cost)
- FSM-based player

---

## Stats
- STR: damage
- AGI: move + anim speed
- VIT: HP
- STA: stamina

### Level
- Exp: 100 + (lvl-1)*50
- Level up → stat UI (pause)

---

## Monsters (FSM)
- Idle / Chase / Attack / Damaged / Dead
- Types: melee / elite / ranged
- SO data: HP, ATK, speed, ranges, cooldown, stagger, exp

---

## Boss (Done)
- BT (Behavior Tree) — custom implementation
- Blackboard 패턴으로 노드 간 상태 공유
- 트리 구조: ContinueCharge > ContinueTelegraph > (거리 체크 → 패턴결정 → 근접/차지) > Chase
- 근접: 애니메이션 이벤트 + OnAttackHit()
- 차지: Telegraph(예고) → 돌진 → 쿨다운
- 패턴 선택: chargeRange 이상이면 차지, 이하면 랜덤
- BossHealth: IDamageable, OnHpChanged, OnDeath
- BossData SO: HP/ATK/speed/ranges/cooldown/charge 관련 필드
- future: 원거리 공격 (RangedAttackNode 추가 예정)

---

## Player (Done)
- Move (Rigidbody MovePosition)
- combo + heavy
- dodge
- stamina system

---

## Combat System
- hitbox (BoxCollider trigger)
- anim events: enable/disable hitbox
- HashSet = no multi-hit

### Damage
- STR × multiplier
- AttackType enum + Dictionary
- StateMachineBehaviour sync attack type

---

## Wave System
Flow:
- Prep → Spawn → Fight → Clear → End

Features:
- WaveData SO
- spawn points round-robin
- shared prefab + inject data
- alive tracking via death callback
- events (start/clear/alive/all clear)
- debug: kill all

---

## UI
- stat distribution panel (level up)
- Time.timeScale = 0 pause
- cursor unlock
- ExpBar: Slider + 레벨 텍스트, OnExpChanged/OnLevelUp 구독
- GoldHud: OnGoldChanged 구독, "Gold: {amount}" 표시
- PotionHud: 아이콘 Image fillAmount(쿨타임), TMP_Text(current/max)

---

## Gold & Exp (Done)
- GoldSystem: AddGold / SpendGold, OnGoldChanged event
- MonsterController HandleDeath → gold.AddGold(data.goldReward)
- ExperienceSystem 기존 구현, ExpBar UI 신규 추가

---

## Potion System (Done)
- PotionSystem: maxPotions=2, healPercent=0.3, cooldown=3f
- UsePotion: HP 풀이면 막음, 쿨타임, 파티클 재생
- WaveManager.OnWaveCleared → RestoreAll()
- PlayerInputHandler: OnPotion 이벤트 추가
- PotionHud: fillAmount 쿨타임, current/max 텍스트

---

## Map
- 임포트한 에셋의 Demo 씬에서 Terrain 추출해서 임시 사용
- Terrain 평탄화: Paint Height 도구 활용

---

## TODO
- 보스 원거리 공격 패턴 추가 (RangedAttackNode)
- anim speed per attack type (not global)
- remove debug inputs (Test.cs, HealthSystem J키)
- animation event cleanup
- 포션 파티클 에셋 적용
- pooling (optional)
- 상점 시스템 (AddMaxPotion 연동) ✓
- 보스 웨이브 스폰: WaveData에 bossPrefabOverride 추가, BossHealth.OnDeath로 alive 추적 (방식 1번)

---

## Tech
- Unity URP
- InputSystem
- FSM (player/monster), BT (boss)
- ScriptableObject-driven

---

## Save
- stage progress
- stats
- skill upgrades

---

## Architecture
- input / state / physics / animation separated
- FSM = gating
- physics = MovePosition driven
- combat = animation event driven

---

## Roadmap
W1: core combat + exp + monsters + boss + 5 waves  
W2: skills + shop + boss patterns + 10 waves + NavMesh (optional)  
W3: polish + balance + bugfix

---

## Claude Rule
- architecture/debug only
- no file creation — show skeleton code in chat, user types it in
- guide step-by-step: explain structure → show skeleton → advise on what to fill in
- no full rewrites
- response: minimal tokens, no padding, no summary, answer first

---

## Asset
- UnityChanToonShader URP