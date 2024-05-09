using UnityEngine;
using UnityEngine.AI;

public class TaskFollow : BehaviorNode
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _playerTransform;
    private readonly Transform _transform;
    private readonly SpriteRenderer _sprite;
    private const float TimeUpdate = 0.5f;

    private float _timeLeft;

    public TaskFollow(NavMeshAgent agent, SpriteRenderer sprite, Transform transform, Transform playerTransform)
    {
        _agent = agent;
        _sprite = sprite;
        _playerTransform = playerTransform;
        _transform = transform;
    }

    public override BehaviorNodeState Evaluate()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            _sprite.flipX = _playerTransform.position.x < _transform.position.x;
            _agent.SetDestination(_playerTransform.position);
            _timeLeft = TimeUpdate;
        }

        state = BehaviorNodeState.Running;
        return state;
    }
}
