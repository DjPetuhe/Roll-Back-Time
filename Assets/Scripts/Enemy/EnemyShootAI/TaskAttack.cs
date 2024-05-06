using UnityEngine;
using UnityEngine.AI;

public class TaskAttack : BehaviorNode
{
    private readonly NavMeshAgent _agent;
    private readonly GameObject _bulletPrefab;
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly int _damage;
    private readonly float _range;
    private readonly float _timeBetweenShots;
    private readonly float _bulletSpeed;

    private float _timeLeft;

    public TaskAttack(
        NavMeshAgent agent,
        GameObject bulletPrefab,
        Transform transform,
        Transform playerTransform,
        int damage,     
        float range,
        float timeBetweenShots,
        float bulletSpeed
    )
    {
        _agent = agent;
        _bulletPrefab = bulletPrefab;
        _transform = transform;
        _playerTransform = playerTransform;
        _damage = damage;
        _range = range;
        _timeBetweenShots = timeBetweenShots;
        _bulletSpeed = bulletSpeed;
    }


    public override BehaviorNodeState Evaluate()
    {
        if (_agent.hasPath)
            _agent.ResetPath();

        if (_timeLeft <+ 0)
        {
            _timeLeft = _timeBetweenShots;
            Vector3 direction = (_playerTransform.position - _transform.position).normalized;
            GameObject bullet = Object.Instantiate(_bulletPrefab, _transform.position + direction, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Direction = direction;
            bulletScript.Speed = _bulletSpeed;
            bulletScript.Damage = _damage;
            bulletScript.Range = _range;
            bulletScript.KnockBack = 100; //temporary?
            bulletScript.State = EnvironemtnState.Hostile;
        }

        _timeLeft -= Time.deltaTime;

        state = BehaviorNodeState.Running;
        return state;
    }
}
