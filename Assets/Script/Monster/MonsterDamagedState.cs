using UnityEngine;

public class MonsterDamagedState : IMonsterState
{
    private float exitTime;

    public void Enter(MonsterController ctx)
    {
        ctx.Anim.SetTrigger(MonsterController.DamageHash);
        exitTime = Time.time + ctx.Data.staggerDuration;
    }

    public void Tick(MonsterController ctx)
    {
        if (Time.time >= exitTime)
            ctx.ChangeState(new MonsterChaseState());
    }

    public void Exit(MonsterController ctx) { }
}