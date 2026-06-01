using UnityEngine;

public class MonsterIdleState : IMonsterState
{
    public void Enter(MonsterController ctx)
    {
        ctx.Anim.SetFloat(MonsterController.MoveHash, 0);
    }

    public void Tick(MonsterController ctx)
    {
        if (ctx.Target == null) return;

        float distance = Vector3.Distance(ctx.transform.position, ctx.Target.position);
        if (distance < ctx.Data.detectRange)
            ctx.ChangeState(new MonsterChaseState());
    }

    public void Exit(MonsterController ctx) { }
}