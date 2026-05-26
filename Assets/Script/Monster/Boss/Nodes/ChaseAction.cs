using UnityEngine;

public class ChaseAction : BTNode
{
    public override NodeState Execute(BossBlackboard bb)
    {
        Vector3 dir = (bb.target.position - bb.self.position);
        dir.y = 0;
        if (dir != Vector3.zero)
            bb.self.rotation = Quaternion.LookRotation(dir);

        bb.self.position = Vector3.MoveTowards(bb.self.position, 
            bb.target.position, bb.data.moveSpeed * Time.deltaTime);
        bb.anim.SetFloat("Move", 1f);
        return NodeState.Running;
    }
}