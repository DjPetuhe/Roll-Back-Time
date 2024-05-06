using UnityEngine;
using UnityEngine.AI;

public class TaskFollow : BehaviorNode
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _playerTransform;
    private const float TimeUpdate = 0.5f;

    private float _timeLeft;

    public TaskFollow(NavMeshAgent agent, Transform playerTransform)
    {
        _agent = agent;
        _playerTransform = playerTransform;
    }

    public override BehaviorNodeState Evaluate()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            _agent.SetDestination(_playerTransform.position);
            _timeLeft = TimeUpdate;
        }

        state = BehaviorNodeState.Running;
        return state;
    }
}
