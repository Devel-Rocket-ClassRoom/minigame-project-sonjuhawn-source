using UnityEngine;

public class ChargeAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (bb.nextAttackPattern != 1) return NodeState.Failure;

        bb.isTelegraphing = true;
        bb.telegraphEndTime = Time.time + bb.data.telegraphDuration;
        bb.anim.SetTrigger("ChargeReady");
        return NodeState.Success;
    }
}