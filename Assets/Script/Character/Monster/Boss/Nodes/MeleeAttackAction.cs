using UnityEngine;

public class MeleeAttackAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.nextAttackPattern != 0) return NodeState.Failure;

        bb.anim.SetTrigger("Attack");

        bb.isAttackCooldown = true;
        bb.onAttackFired?.Invoke();
        return NodeState.Success;
    }
}