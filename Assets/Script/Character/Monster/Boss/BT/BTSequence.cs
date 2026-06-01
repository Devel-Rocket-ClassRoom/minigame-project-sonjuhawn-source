using System.Collections.Generic;

public class BTSequence : BTNode
{
    private List<BTNode> children;

    public BTSequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Execute(BossBlackboard bb)
    {
        foreach(var child in children)
        {
            var result =  child.Execute(bb);
            if (result != NodeState.Success)
                return result;
        }
        return NodeState.Success;
    }
}