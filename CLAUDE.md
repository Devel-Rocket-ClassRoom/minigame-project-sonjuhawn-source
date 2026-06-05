# Project Summary (Ultra Compact)

## 완성 (최종 빌드)
- BDO-style action hack & slash, 5웨이브 + 보스
- 포폴 목적, Unity URP

---

## Combat
- LMB 3타 콤보 / RMB 강공 / Space 회피(입력방향) / Finisher 점프
- InputSystem, 콤보버퍼, 스태미나, FSM-based player
- hitbox trigger + anim event, STR×multiplier, AttackType Dictionary

---

## Stats
- STR: damage / AGI: move+anim speed / VIT: HP / STA: stamina
- 레벨업 → 스탯 분배 UI (pause)

---

## Monsters
- FSM: Idle/Chase/Attack/Damaged/Dead
- melee / elite(telegraph) / ranged
- MonsterData SO 기반

---

## Boss
- BT (custom) + Blackboard
- 패턴: 근접 / 차지(telegraph→돌진) / 원거리(투사체)
- 거리 기반 패턴 선택, 등장 애니메이션, HP바(첫 피격 시 표시)

---

## Wave
- WaveData SO, Prep→Spawn→Fight→Clear→Rest(상점)→Next
- 마지막 웨이브 후 보스 스폰 (bossPrefabOverride)

---

## Systems
- GoldSystem / ExperienceSystem / PotionSystem / StaminaSystem
- ShopSystem: 포션+1 / HP회복 / 랜덤스탯강화
- PauseManager: pauseCount 기반 다중 UI 관리
- AudioManager: BGM(일반/보스) / SFX 싱글톤
- DamagePopup: 스크린스페이스 데미지 숫자

---

## UI
- 시작 / 일시정지(ESC) / 게임오버 / 클리어(처치수/골드/시간)
- 옵션(감도/BGM/SFX/음소거) / 상점 / 스탯분배 / 튜토리얼(ScrollView)
- HpBar / StaminaBar / ExpBar / GoldHud / PotionHud / BossHpBar / ToastManager

---

## Architecture
- input / state / physics / animation separated
- FSM = gating / physics = MovePosition / combat = anim event driven
- ScriptableObject-driven

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