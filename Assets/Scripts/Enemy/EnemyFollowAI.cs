using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowAI : MonoBehaviour
{
    [field: Header("Agent")]
    [field: SerializeField]
    public NavMeshAgent Agent { get; private set; }

    [field:Header("Stats")]
    [field: SerializeField]
    public float Speed { get; private set; }
    [field: SerializeField]
    public int Damage { get; private set; }

    [field: Header("Components")]
    [field: SerializeField]
    public SpriteRenderer Sprite { get; private set; }
    [field:SerializeField]
    public Animator Animator { get; private set; }

    public GameObject _player { get; private set; }
    public PlayerHealthControl _playerHealth { get; private set; }

    protected virtual void Start()
    {
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        Agent.speed = Speed;

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<PlayerHealthControl>();
        InvokeRepeating(nameof(UpdatePath), 0, 0.5f);
    }

    private void UpdatePath()
    {
        if (!_player)
            return;
        Sprite.flipX = _player.transform.position.x < transform.position.x;
        Agent.SetDestination(_player.transform.position);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _playerHealth.State == PlayerState.Normal)
        {
            Animator.SetTrigger("Attack");
            _playerHealth.DealDamage(Damage);
        }
    }
}
