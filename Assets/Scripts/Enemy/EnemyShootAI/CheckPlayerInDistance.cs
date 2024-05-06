using UnityEngine;

public class CheckPlayerInDistance : BehaviorNode
{
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly float _distance;

    public CheckPlayerInDistance(Transform transform, Transform playerTransform, float distance)
    {
        _transform = transform;
        _playerTransform = playerTransform;
        _distance = distance;
    }

    public override BehaviorNodeState Evaluate()
    {
        if (Vector3.Distance(_transform.position, _playerTransform.position) <= _distance)
        {
            state = BehaviorNodeState.Succes;
            return state;
        }
        state = BehaviorNodeState.Failure;
        return state;
    }
}
