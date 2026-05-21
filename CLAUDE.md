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
* up to 10 waves

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
