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

    [Header("Components")]
    [SerializeField] Animator Animator;
    [SerializeField] SpriteRenderer Sprite;

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
        BehaviorNode root = new Sequence(new()
        {
            new CheckPlayerExistance(player),
            new Selector(new()
            {
                new Sequence(new()
                {
                    new CheckPlayerAvailability(transform, player.transform),
                    new CheckPlayerInDistance(transform, player.transform, AttackDistance),
                    new Selector(new()
                    {
                        new Sequence(new()
                        {
                            new CheckPlayerInDistance(transform, player.transform, RetreatDistance),
                            new TaskRetreat(Agent, transform, player.transform),
                            new TaskAttack(this, Agent, BulletPrefab, Animator, Sprite, transform, player.transform, Damage, Range, TimeBetweenShots, BulletSpeed, false)
                        }),
                        new TaskAttack(this, Agent, BulletPrefab, Animator, Sprite, transform, player.transform, Damage, Range, TimeBetweenShots, BulletSpeed)
                    })
                }),
                new TaskFollow(Agent, Sprite, transform, player.transform)
            })
        });
        return root;
    }
}
