using UnityEngine;

public class MonsterTelegraphAttackState : IMonsterState
{
    private float chargeEndTime;
    private bool fired;

    public void Enter(MonsterController ctx)
    {
        ctx.FacePlayer();
        ctx.Anim.SetTrigger(MonsterController.ChargeHash);          // 3초 대기모션
        chargeEndTime = Time.time + ctx.Data.telegraphTime;
        fired = false;
    }

    public void Tick(MonsterController ctx)
    {
        if (ctx.Target == null) return;   // ← 추가
        ctx.FacePlayer();

        float distance = Vector3.Distance(ctx.transform.position, ctx.Target.position);

        // 거리 벌리면 캔슬 (회피 성공)
        if (!fired && distance > ctx.Data.attackRange + 1.5f)   // 회피 여유 거리
        {
            ctx.ChangeState(new MonsterChaseState());
            return;
        }

        if (!fired && Time.time >= chargeEndTime)
        {
            fired = true;
            ctx.Anim.SetTrigger(MonsterController.HeavyHash);       // anim event에서 OnHeavyAttackHit 호출
            chargeEndTime = Time.time + ctx.Data.heavyRecoveryTime; // 변수 재활용

        }

        // 강공 애니 끝나면 다음 사이클은 cooldown으로 — 간단하게 chase로 복귀
        if (fired && Time.time >= chargeEndTime)
            ctx.ChangeState(new MonsterChaseState());
    }

    public void Exit(MonsterController ctx)
    {
        if (!fired && !ctx.Health.IsDead)
        {
            ctx.Anim.ResetTrigger(MonsterController.ChargeHash);   // 잔여 Charge도 청소
            ctx.Anim.SetTrigger(MonsterController.CancelHash);
        }
    }
}