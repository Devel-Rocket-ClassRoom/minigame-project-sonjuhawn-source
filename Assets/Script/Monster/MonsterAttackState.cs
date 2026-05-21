using UnityEngine;

public class MonsterAttackState : IMonsterState
{
    private float nextAttackTime;

    public void Enter(MonsterController ctx) 
    {
        ctx.FacePlayer();
        ctx.Anim.SetTrigger(MonsterController.AttackHash);
        nextAttackTime = Time.time + ctx.Data.attackCooldown;
    }
    public void Tick(MonsterController ctx) 
    {
        ctx.FacePlayer();
        if (Time.time < nextAttackTime) return;   // 쿨다운 중이면 그냥 대기

        float distance = Vector3.Distance(ctx.transform.position, ctx.Target.position);
        if (distance > ctx.Data.attackRange)
        {
            ctx.ChangeState(new MonsterChaseState());
            return;
        }

        ctx.Anim.SetTrigger(MonsterController.AttackHash);
        nextAttackTime = Time.time + ctx.Data.attackCooldown;
    }
    public void Exit(MonsterController ctx) { }
}