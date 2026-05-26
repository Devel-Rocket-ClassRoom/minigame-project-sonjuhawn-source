using UnityEngine;

public class DecidePatternAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.isAttackCooldown) return NodeState.Failure;

        float distance = Vector3.Distance(bb.self.position, bb.target.position);

        // 차징 범위 밖이면 차징, 안이면 근접
        if (distance > bb.data.chargeRange)
            bb.nextAttackPattern = 1; // 차징
        else
            bb.nextAttackPattern = Random.Range(0, 2); // 근접

        return NodeState.Success;
    }
}