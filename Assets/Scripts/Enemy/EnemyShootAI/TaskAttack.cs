using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TaskAttack : BehaviorNode
{
    private readonly BehaviorTree _tree;
    private readonly NavMeshAgent _agent;
    private readonly GameObject _bulletPrefab;
    private readonly Animator _animator;
    private readonly SpriteRenderer _sprite;
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly int _damage;
    private readonly float _range;
    private readonly float _timeBetweenShots;
    private readonly float _bulletSpeed;
    private readonly bool _stand;

    private float _timeLeft;

    public TaskAttack(
        BehaviorTree tree,
        NavMeshAgent agent,
        GameObject bulletPrefab,
        Animator animator,
        SpriteRenderer sprite,
        Transform transform,
        Transform playerTransform,
        int damage,     
        float range,
        float timeBetweenShots,
        float bulletSpeed,
        bool stand = true
    )
    {
        _tree = tree;
        _agent = agent;
        _bulletPrefab = bulletPrefab;
        _animator = animator;
        _sprite = sprite;
        _transform = transform;
        _playerTransform = playerTransform;
        _damage = damage;
        _range = range;
        _timeBetweenShots = timeBetweenShots;
        _bulletSpeed = bulletSpeed;
        _stand = stand;
    }


    public override BehaviorNodeState Evaluate()
    {
        if (_stand && _agent.hasPath)
            _agent.ResetPath();

        if (_timeLeft <+ 0)
        {
            _timeLeft = _timeBetweenShots;
            Vector3 direction = (_playerTransform.position - _transform.position).normalized;
            _tree.StartCoroutine(Attack(direction));
        }

        _timeLeft -= Time.deltaTime;

        state = BehaviorNodeState.Running;
        return state;
    }

    private IEnumerator Attack(Vector3 dir)
    {
        _animator.SetTrigger("Attack 3");
        _sprite.flipX = _playerTransform.position.x < _transform.position.x;

        yield return new WaitForSeconds(0.2f);
        GameObject bullet = Object.Instantiate(_bulletPrefab, _transform.position + dir, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Direction = dir;
        bulletScript.Speed = _bulletSpeed;
        bulletScript.Damage = _damage;
        bulletScript.Range = _range;
        bulletScript.KnockBack = 100; //temporary?
        bulletScript.State = EnvironemtnState.Hostile;
    }
}
