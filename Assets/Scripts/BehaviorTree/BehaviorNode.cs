using System.Collections.Generic;

public class BehaviorNode
{
    public BehaviorNode Parent { get; protected set; }
    protected List<BehaviorNode> Childs = new();
    protected BehaviorNodeState state;

    public BehaviorNode() => Parent = null;

    public BehaviorNode(List<BehaviorNode> childs)
    {
        foreach (BehaviorNode child in childs)
        {
            child.Parent = this;
            Childs.Add(child);
        }
    }

    public virtual BehaviorNodeState Evaluate() => BehaviorNodeState.Succes;
}
