# Week1 Environment Context

## 몬스터
- 근거리 추적 AI
- 공격 + HP
- 처치 시 경험치/골드 지급
- MonsterData(SO) 사용 예정

## 보스
- 플레이어 추적
- 근접 공격
- 5웨이브 후 등장
- 처치 시 클리어

## 맵
- 단일 맵
- 플레이어 시작 지점
- 몬스터 스폰 포인트 4~6개
- 보스 스폰 위치

## 웨이브
- 5웨이브 구성
- 몬스터 전멸 시 다음 웨이브
- 웨이브 종료 후 보스 등장
- 현재 웨이브 / 남은 몬스터 UI 표시 예정

## 구현 예정 구조
- MonsterData / BossData / WaveData : ScriptableObject
- WaveManager : 웨이브 진행 관리
- IDamageable 인터페이스 고려 중

## 현재 우선순위
1. 플레이어 전투 안정화
2. 몬스터 AI
3. 웨이브 시스템
4. 보스
5. 빌드