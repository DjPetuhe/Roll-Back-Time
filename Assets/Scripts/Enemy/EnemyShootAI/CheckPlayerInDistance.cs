using UnityEngine;

public class CheckPlayerInDistance : BehaviorNode
{
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly float _distance;
    private readonly bool _reverse;

    public CheckPlayerInDistance(Transform transform, Transform playerTransform, float distance, bool reverse = false)
    {
        _transform = transform;
        _playerTransform = playerTransform;
        _distance = distance;
        _reverse = reverse;
    }

    public override BehaviorNodeState Evaluate()
    {
        float d = Vector3.Distance(_transform.position, _playerTransform.position);
        if ((_reverse && d >= _distance) || (!_reverse && d <= _distance))
        {
            state = BehaviorNodeState.Succes;
            return state;
        }
        state = BehaviorNodeState.Failure;
        return state;
    }
}
