using UnityEngine;

public class MonsterChaseState : IMonsterState
{
    public void Enter(MonsterController ctx) 
    {
        ctx.Anim.SetFloat(MonsterController.MoveHash, 1);
    }
    public void Tick(MonsterController ctx) 
    {
        if (ctx.Target == null) return;

        float distance = Vector3.Distance(ctx.transform.position, ctx.Target.position);
        if (distance > ctx.Data.detectRange)
            ctx.ChangeState(new MonsterIdleState());
        else if (distance <= ctx.Data.attackRange)
            ctx.ChangeState(ctx.CreateAttackState());   // 기존: new MonsterAttackState()
        else
            ctx.ChasePlayer();

    }
    public void Exit(MonsterController ctx)
    {
        ctx.Anim.SetFloat(MonsterController.MoveHash, 0);
    }
}