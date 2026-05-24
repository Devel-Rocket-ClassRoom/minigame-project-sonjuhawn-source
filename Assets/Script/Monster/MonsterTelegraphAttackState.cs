using UnityEngine;

public class MonsterTelegraphAttackState : IMonsterState
{
    private static readonly int ChargeHash = Animator.StringToHash("Charge");
    private static readonly int HeavyHash = Animator.StringToHash("HeavyAttack");

    private float chargeEndTime;
    private bool fired;

    public void Enter(MonsterController ctx)
    {
        ctx.FacePlayer();
        ctx.Anim.SetTrigger(ChargeHash);          // 3초 대기모션
        chargeEndTime = Time.time + ctx.Data.telegraphTime;
        fired = false;
    }

    public void Tick(MonsterController ctx)
    {
        ctx.FacePlayer();                         // 차징 중 천천히 추적하려면 여기서 살짝 회전만

        float distance = Vector3.Distance(ctx.transform.position, ctx.Target.position);

        // 거리 벌리면 캔슬 (회피 성공)
        if (distance > ctx.Data.attackRange + 1.5f)   // 회피 여유 거리
        {
            ctx.ChangeState(new MonsterChaseState());
            return;
        }

        if (!fired && Time.time >= chargeEndTime)
        {
            fired = true;
            ctx.Anim.SetTrigger(HeavyHash);       // anim event에서 OnHeavyAttackHit 호출
        }

        // 강공 애니 끝나면 다음 사이클은 cooldown으로 — 간단하게 chase로 복귀
        if (fired && Time.time >= chargeEndTime + ctx.Data.attackCooldown)
            ctx.ChangeState(new MonsterChaseState());
    }

    public void Exit(MonsterController ctx) { }
}