using System.Collections.Generic;

public class Sequence : BehaviorNode
{
    public Sequence() : base() { }
    public Sequence(List<BehaviorNode> children) : base(children) { }

    public override BehaviorNodeState Evaluate()
    {
        bool hasRunningChild = false;

        foreach (var child in Childs)
        {
            switch (child.Evaluate())
            {
                case BehaviorNodeState.Failure:
                    state = BehaviorNodeState.Failure;
                    return state;
                case BehaviorNodeState.Succes:
                    continue;
                case BehaviorNodeState.Running:
                    hasRunningChild = true;
                    continue;
                default:
                    state = BehaviorNodeState.Succes;
                    return state;
            }
        }

        state = hasRunningChild ? BehaviorNodeState.Running : BehaviorNodeState.Succes;
        return state;
    }

}
