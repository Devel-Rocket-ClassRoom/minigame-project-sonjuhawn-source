using UnityEngine;
public class RangedAttackAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.nextAttackPattern != 2) return NodeState.Failure;

        bb.anim.SetTrigger("RangedAttack"); 
        bb.isAttackCooldown = true;
        bb.isRangedCooldown = true;
        bb.onAttackFired?.Invoke();
        bb.onRangedFired?.Invoke();
        return NodeState.Success;
    }
}