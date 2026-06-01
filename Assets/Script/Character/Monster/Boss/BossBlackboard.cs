using UnityEngine;

public class BossBlackboard
{
    public Transform self;      // 보스 본인
    public Transform target;    // 플레이어
    public Animator anim;
    public BossData data;       // BossData 

    public int nextAttackPattern;

    public bool isTelegraphing;
    public float telegraphEndTime;

    public float chargeEndTime;  // 돌진 종료 시간
    public Vector3 chargeDir;    // 돌진 방향 (고정)

    public bool isChargeCooldown;
    public System.Action onChargeFired;
    public System.Action onAttackFired;

    public bool isAttackCooldown;   // 공격 쿨다운 중?
    public bool isCharging;         // 차징 중?

    public bool isRangedCooldown;
    public System.Action onRangedFired;
}