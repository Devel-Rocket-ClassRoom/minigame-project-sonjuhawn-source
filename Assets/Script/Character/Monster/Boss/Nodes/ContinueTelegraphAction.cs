using UnityEngine;

public class ContinueTelegraphAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        if (!bb.isTelegraphing) return NodeState.Failure;

        if (Time.time >= bb.telegraphEndTime)
        {
            bb.isTelegraphing = false;
            bb.isCharging = true;
            bb.isChargeCooldown = true;
            bb.onChargeFired?.Invoke();
            bb.chargeDir = (bb.target.position - bb.self.position).normalized;
            bb.chargeEndTime = Time.time + bb.data.chargeDuration;
            bb.anim.SetTrigger("Charge");
            return NodeState.Success;
        }

        Vector3 dir = (bb.target.position - bb.self.position);
        dir.y = 0;
        if (dir != Vector3.zero)
            bb.self.rotation = Quaternion.LookRotation(dir);

        return NodeState.Running; // 대기 중 — 이동 없음
    }
}