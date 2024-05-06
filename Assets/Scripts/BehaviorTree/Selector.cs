using System.Collections.Generic;

public class Selector : BehaviorNode
{
    public Selector() : base() { }
    public Selector(List<BehaviorNode> children) : base(children) { }

    public override BehaviorNodeState Evaluate()
    {
        foreach (var child in Childs)
        {
            switch (child.Evaluate())
            {
                case BehaviorNodeState.Failure:
                    continue;
                case BehaviorNodeState.Succes:
                    state = BehaviorNodeState.Succes;
                    return state;
                case BehaviorNodeState.Running:
                    state = BehaviorNodeState.Running;
                    return state;
                default:
                    continue;
            }
        }

        state = BehaviorNodeState.Failure;
        return state;
    }

}
