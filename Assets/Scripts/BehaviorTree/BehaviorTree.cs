using UnityEngine;
using System.Collections.Generic;

public abstract class BehaviorTree : MonoBehaviour
{
    private BehaviorNode _root = null;

    protected virtual void Start() => _root = SetupTree();

    private void Update() => _root?.Evaluate();

    protected abstract BehaviorNode SetupTree();
}