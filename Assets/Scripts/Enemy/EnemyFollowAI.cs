using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowAI : MonoBehaviour
{
    [Header("Agent")]
    [SerializeField] NavMeshAgent Agent;

    [Header("Stats")]
    [SerializeField] float Speed;
    [SerializeField] int Damage;

    private GameObject _player;
    private PlayerHealthControl _playerHealth;

    private void Start()
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
        Agent.SetDestination(_player.transform.position);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _playerHealth.State == PlayerState.Normal)
            _playerHealth.DealDamage(Damage);
    }
}
