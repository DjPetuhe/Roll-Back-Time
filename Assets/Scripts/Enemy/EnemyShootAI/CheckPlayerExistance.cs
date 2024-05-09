using UnityEngine;

public class CheckPlayerExistance : BehaviorNode
{
    private readonly GameObject _player;

    public CheckPlayerExistance(GameObject player) => _player = player;

    public override BehaviorNodeState Evaluate()
    {
        if (_player != null)
        {
            state = BehaviorNodeState.Succes;
            return state;
        }
        state = BehaviorNodeState.Failure;
        return state;
    }
}
