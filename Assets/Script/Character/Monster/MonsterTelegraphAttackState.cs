using UnityEngine;

public class MonsterTelegraphAttackState : IMonsterState
{
    private float chargeEndTime;
    private bool fired;

    public bool IsCharging => !fired;

    public void Enter(MonsterController ctx)
    {
        ctx.FacePlayer();
        ctx.Anim.SetTrigger(MonsterController.ChargeHash);
        chargeEndTime = Time.time + ctx.Data.telegraphTime;
        fired = false;
    }

    public void Tick(MonsterController ctx)
    {
        if (ctx.Target == null)
            return;

        ctx.FacePlayer();

        float distance = Vector3.Distance(ctx.transform.position, ctx.Target.position);

        if (!fired && distance > ctx.Data.attackRange + 1.5f)
        {
            ctx.ChangeState(new MonsterChaseState());
            return;
        }

        if (!fired && Time.time >= chargeEndTime)
        {
            fired = true;
            ctx.Anim.SetTrigger(MonsterController.HeavyHash);
            chargeEndTime = Time.time + ctx.Data.heavyRecoveryTime;
        }

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