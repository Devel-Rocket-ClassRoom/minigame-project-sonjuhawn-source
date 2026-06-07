using UnityEngine;

public class DecidePatternAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.isAttackCooldown) return NodeState.Failure;

        float distance = Vector3.Distance(bb.self.position, bb.target.position);

        // 거리 구간별 패턴 결정 (0=근접, 1=차징, 2=원거리)
        if (distance <= bb.data.attackRange)
        {
            // 근접 사거리: 80% 근접, 20% 차징
            bb.nextAttackPattern = Random.value < 0.8f ? 0 : 1;
        }
        else if (distance >= bb.data.chargeRange)
        {
            // 먼 거리: 50% 차징, 50% 원거리
            bb.nextAttackPattern = Random.value < 0.5f ? 1 : 2;
        }
        else
        {
            // 중간 거리: 60% 원거리, 40% 차징
            bb.nextAttackPattern = Random.value < 0.6f ? 2 : 1;
        }

        // 쿨다운 중인 패턴 뽑히면 근접으로 fallback
        if (bb.nextAttackPattern == 1 && bb.isChargeCooldown)
            bb.nextAttackPattern = 0;
        if (bb.nextAttackPattern == 2 && bb.isRangedCooldown)
            bb.nextAttackPattern = 0;

        return NodeState.Success;
    }
}