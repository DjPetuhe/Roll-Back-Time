using UnityEngine.AI;
using UnityEngine;

public class TaskRetreat : BehaviorNode
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private const float TimeUpdate = 0.5f;

    private float _timeLeft;

    public TaskRetreat(NavMeshAgent agent, Transform transform, Transform playerTransform)
    {
        _agent = agent;
        _transform = transform;
        _playerTransform = playerTransform;
    }

    public override BehaviorNodeState Evaluate()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            Vector3 directionToPlayer = _playerTransform.position - _transform.position;
            _agent.SetDestination(_transform.position - directionToPlayer.normalized);
            _timeLeft = TimeUpdate;
        }

        state = BehaviorNodeState.Running;
        return state;
    }
}
