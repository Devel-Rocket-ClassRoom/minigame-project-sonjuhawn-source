using UnityEngine;

public class DecidePatternAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.isAttackCooldown) return NodeState.Failure;

        bb.nextAttackPattern = Random.Range(0,3); // 0=근접, 1=차징, 2=원거리

        // 쿨다운 중인 패턴 뽑히면 근접으로 fallback
        if (bb.nextAttackPattern == 1 && bb.isChargeCooldown)
            bb.nextAttackPattern = 0;
        if (bb.nextAttackPattern == 2 && bb.isRangedCooldown)
            bb.nextAttackPattern = 0;

        return NodeState.Success;
    }
}