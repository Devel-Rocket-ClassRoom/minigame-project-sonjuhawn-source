using UnityEngine;

public class CheckDistance : BTNode
{
    private float threshold;
    private bool within; // true = 이 거리 안에 있으면 Success

    public CheckDistance(float threshold, bool within = true)
    {
        this.threshold = threshold;
        this.within = within;
    }

    public override NodeState Execute(BossBlackboard bb)
    {
        var distance = Vector3.Distance(bb.self.position, bb.target.position);
        if (within)
        {
            if (distance <= threshold)
                return NodeState.Success;
            else 
                return NodeState.Failure;
        }
        else
        {
            if (distance > threshold)
                return NodeState.Success;
            else
                return NodeState.Failure;
        }
    }
}