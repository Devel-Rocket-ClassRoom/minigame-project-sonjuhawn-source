# Decisions Summary

## 현재 아키텍처

- InputSystem 기반
- PlayerInputHandler / CharacterMover / PlayerCombat 책임 분리
- Animator 기반 상태 동기화
- PlayerState enum 사용
- 콤보 인덱스는 Animator StateMachineBehaviour에서 관리

## 스탯 시스템

- 현재:
  - PlayerStats int 기반 MVP
  - IStatProvider 인터페이스 사용

- 추후:
  - SO + Modifier 시스템 예정

## 전투 정책

- 기본공격:
  - 스태미나 회복
  - Animation Event 타이밍 사용

- 강공:
  - 쿨타임 없음
  - 스태미나만 소모

- 회피:
  - i-frame 존재
  - 짧은 쿨타임 사용

## 몬스터/웨이브

- 몬스터 데이터는 ScriptableObject 사용
- 드랍 오브젝트 없이 즉시 지급
- 5웨이브 후 보스 등장

## 현재 우선순위

1. 플레이어 전투 안정화
2. 몬스터 AI
3. 웨이브 시스템
4. 보스
5. 빌드

## 추후 리팩토링 예정

- Stat Modifier 시스템
- CSV 데이터 파이프라인
- 데미지 팝업
- 스킬 강화 UI