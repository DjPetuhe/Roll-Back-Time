using UnityEngine;
using UnityEngine.AI;

public class EnemyShootBehaviorTree : BehaviorTree
{
    [Header("Agent")]
    [SerializeField] NavMeshAgent Agent;

    [Header("Prefab")]
    [SerializeField] GameObject BulletPrefab;

    [Header("Stats")]
    [SerializeField] float Speed;
    [SerializeField] int Damage;
    [SerializeField] float Range;
    [SerializeField] float TimeBetweenShots;
    [SerializeField] float BulletSpeed;

    [Header("Distances")]
    [SerializeField] float AttackDistance;
    [SerializeField] float RetreatDistance;

    protected override void Start()
    {
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        Agent.speed = Speed;
        base.Start();
    }

    protected override BehaviorNode SetupTree()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        BehaviorNode root = new Selector(new()
        {
            new Sequence(new()
            {
                new CheckPlayerInDistance(transform, player.transform, RetreatDistance),
                new TaskRetreat(Agent, transform, player.transform)
            }),
            new Sequence(new()
            {
                new CheckPlayerInDistance(transform, player.transform, AttackDistance),
                new TaskAttack(Agent, BulletPrefab, transform, player.transform, Damage, Range, TimeBetweenShots, BulletSpeed)
            }),
            new TaskFollow(Agent, player.transform)
        });

        return root;
    }
}
