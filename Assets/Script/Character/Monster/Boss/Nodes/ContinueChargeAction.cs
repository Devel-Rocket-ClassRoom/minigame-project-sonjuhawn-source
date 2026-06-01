using UnityEngine;

public class ContinueChargeAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (!bb.isCharging) return NodeState.Failure;

        if (Time.time >= bb.chargeEndTime)
        {
            bb.isCharging = false;
            bb.isAttackCooldown = true;
            bb.onAttackFired?.Invoke();
            return NodeState.Success;
        }
        bb.self.position = Vector3.MoveTowards(
            bb.self.position,
            bb.self.position + bb.chargeDir,
            bb.data.chargeSpeed * Time.deltaTime);
        return NodeState.Running;
    }
}