# Minigame Project Summary (for Claude token saving)

## Genre

* Black Desert-style action hack-and-slash
* Single-map wave clear structure
* 5~10 stages + final boss

---

## Core Combat

### Inputs

* LMB: 3-hit basic combo

  * restores stamina
* RMB: heavy combo

  * consumes stamina
  * no cooldown
* Space: dodge roll

  * i-frame
  * stamina cost
  * short cooldown (0.3~0.5s)

### Planned Skills

* Q: guard/counter explosion
* Shift+Q: slam + knockdown
* Shift+LMB: spinning attack drain-type

### Combat Design

* InputSystem-based command input
* combo input buffer
* stamina resource loop:

  * basic atk restores
  * heavy/dodge/skills consume
* state machine based player controller

---

## Progression

### Stage Clear Rewards

* stat upgrades
* skill selection/upgrades
* shop access

### Stats

* Strength
* Agility
* Vitality
* Stamina

### Skill Upgrade Examples

* stamina reduction
* cooldown reduction
* attack power
* cast speed
* i-frame increase

---

## Monsters

### Types

1. melee
2. elite/enhanced
3. ranged projectile

### AI

* FSM based
* states:

  * Idle
  * Chase
  * Attack
  * Damaged
  * Dead

### MonsterData SO

Contains:

* HP
* attack
* move speed
* detect range
* attack range
* cooldown
* stagger duration

---

## Boss

* chase player
* melee attack in range
* special pattern if distance maintained

  * instant charge OR ranged stun
* possible random bombs later

---

## Current Implemented

### Player

* movement
* basic combo
* heavy attack
* dodge
* stamina

### Combat System

* sword hit detection:

  * BoxCollider trigger
  * animation event toggle:

    * EnableHitbox
    * DisableHitbox
  * HashSet prevents multi-hit per swing

### Damage

* formula:

  * damage = Strength × multiplier
* AttackType enum + Dictionary mapping
* AttackTypeBehaviour(StateMachineBehaviour)
  syncs attack state with damage type

### Progression / Stats

* ExperienceSystem on Player

  * AddExp / SpendPoint / OnLevelUp / OnExpChanged
  * formula: ExpForLevel = 100 + (level-1) × 50
  * pendingPoints accumulated per level
* MonsterController.HandleDeath drops `MonsterData.expReward` to player
* StatDistributionPanel UI

  * pops on OnLevelUp, pauses game (Time.timeScale = 0)
  * cursor unlocked while open
  * confirm button gated by pendingPoints == 0
* Stat → ability mapping (baseline = 10, bonus from stat-10)

  * Strength → damage (Strength × multiplier; baseline-equivalent)
  * Agility → move speed + anim.speed
  * Vitality → maxHp
  * Stamina → maxStamina + recover amount

---

## TODO / Improvements

* HUD: EXP bar + level display (currently no in-game UI for exp/level)
* AttackType-scoped anim.speed (currently affects Idle/Move too)
* Remove dev debug keys (HealthSystem J, StaminaSystem U/I, MonsterSpawnerDebug M)
* Imported folder anim event ownership (events live in paid asset .anim files)
* Monster object pooling — defer to polish phase. Profile first; only adopt if Instantiate/Destroy shows up as a hotspot. Use `UnityEngine.Pool.ObjectPool<T>` when introduced.

---

## Tech Goals

* InputSystem command inputs
* ScriptableObject data-driven setup
* FSM AI
* Dictionary/List usage
* save system

---

## Save Data

* stage progress
* skill upgrades
* player stats

---

## Structure / Philosophy

* user writes code directly
* Claude only:

  * architecture
  * debugging
  * review
* minimize file reading/token usage
* code delivery: skeleton only (class shell + signatures + TODO comments). No full implementation unless asked.

---

## Roadmap

### Week 1

* core combat
* exp/stats
* basic monsters
* boss chase/basic atk
* UI
* 5 waves

### Week 2

* 3 skills
* skill selection/upgrades
* remaining monsters
* boss skills
* shop
* up to 5~10 waves

### Week 3

* polish
* balance
* bug fixing

---

## Unity/Project Info

* Unity URP
* new InputSystem
* scenes:

  * SampleScene
  * CharacterTestScene
* imported:

  * UnityChanToonShader URP
