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

## Boss
- chase + melee
- distance skill: charge / stun
- future: ranged + bombs

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

Missing:
- EXP bar
- level UI

---

## TODO
- EXP UI
- anim speed per attack type (not global)
- remove debug inputs
- animation event cleanup
- pooling (optional)

---

## Tech
- Unity URP
- InputSystem
- FSM (player/monster)
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