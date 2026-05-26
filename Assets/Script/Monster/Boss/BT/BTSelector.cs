using System.Collections.Generic;

public class BTSelector : BTNode
{
    private List<BTNode> children;

    public BTSelector(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Execute(BossBlackboard bb)
    {
        foreach (var child in children)
        {
            var result = child.Execute(bb);
            if (result != NodeState.Failure)
                return result;
        }
        return NodeState.Failure;
    }
}