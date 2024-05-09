using UnityEngine;

public class CheckPlayerAvailability : BehaviorNode
{
    private readonly Transform _transform;
    private readonly Transform _playerTransform;

    public CheckPlayerAvailability(Transform transform, Transform playerTransform)
    {
        _transform = transform;
        _playerTransform = playerTransform;
    }

    public override BehaviorNodeState Evaluate()
    {
        Vector3 direction = _playerTransform.position - _transform.position;
        RaycastHit2D hit = Physics2D.CircleCast(_transform.position + direction.normalized, 0.2f, direction, direction.magnitude, LayerMask.GetMask("Player", "Default", "Props", "Plants"));
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            state = BehaviorNodeState.Succes;
            return state;
        }
        state = BehaviorNodeState.Failure;
        return state;
    }
}
