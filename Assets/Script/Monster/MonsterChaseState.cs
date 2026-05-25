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

        float attackDist = ctx.Data.projectilePrefab != null
            ? ctx.Data.rangedAttackRange
            : ctx.Data.attackRange;

        if (distance > ctx.Data.detectRange)
        {
            ctx.ChangeState(new MonsterIdleState());
        }
        else if (ctx.Data.kiteDistance > 0f && distance < ctx.Data.kiteDistance)
        {
            ctx.FacePlayer();
            ctx.Retreat();
        }
        else if (distance <= attackDist)
        {
            ctx.ChangeState(ctx.CreateAttackState());
        }
        else
        {
            ctx.ChasePlayer();
        }
    }
    public void Exit(MonsterController ctx)
    {
        ctx.Anim.SetFloat(MonsterController.MoveHash, 0);
    }
}