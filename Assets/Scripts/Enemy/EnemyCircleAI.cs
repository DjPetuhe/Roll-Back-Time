using UnityEngine;
using System.Collections;

public class EnemyCircleAI : EnemyFollowAI
{
    [Header("Prefab")]
    [SerializeField] GameObject BulletPrefab;

    [Header("Stats Circle")]
    [SerializeField] float Range;
    [SerializeField] float BulletSpeed;
    [SerializeField] int ProjectileNumbers;
    [SerializeField] float MinAttackTime;
    [SerializeField] float MaxAttackTime;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(Attack), Random.Range(MinAttackTime, MaxAttackTime));
    }

    private void Attack() => StartCoroutine(nameof(AttackCoroutine));

    private IEnumerator AttackCoroutine()
    {
        Animator.SetTrigger("Ability");
        Sprite.flipX = _player.transform.position.x < transform.position.x;

        yield return new WaitForSeconds(0.2f);

        float angleStep = 360f / ProjectileNumbers;
        Vector3 startPos = new(1, -0.2f);
        for (int i = 0; i < ProjectileNumbers; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * startPos;
            GameObject bullet = Instantiate(BulletPrefab, transform.position + dir, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Direction = dir.normalized;
            bulletScript.Speed = BulletSpeed;
            bulletScript.Damage = Damage;
            bulletScript.Range = Range;
            bulletScript.KnockBack = 100; //temporary?
            bulletScript.State = EnvironemtnState.Hostile;
        }

        Invoke(nameof(Attack), Random.Range(MinAttackTime, MaxAttackTime));
    }
}
