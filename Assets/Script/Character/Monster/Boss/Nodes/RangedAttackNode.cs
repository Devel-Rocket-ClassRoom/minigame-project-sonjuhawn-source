using UnityEngine;
public class RangedAttackAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.isAttackCooldown) return NodeState.Failure;

        float distance = Vector3.Distance(bb.self.position, bb.target.position);

        if (distance > bb.data.chargeRange && !bb.isChargeCooldown)
            bb.nextAttackPattern = 1; // 차징
        else if (distance > bb.data.attackRange)
            bb.nextAttackPattern = bb.isRangedCooldown ? 1 : Random.Range(1, 3); // 차징 or 원거리
        else
            bb.nextAttackPattern = 0; // 근접

        return NodeState.Success;
    }
}