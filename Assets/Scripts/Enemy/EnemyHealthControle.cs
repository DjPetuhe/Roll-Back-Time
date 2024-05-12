using UnityEngine;
using System.Collections;

public class EnemyHealthControle : MonoBehaviour
{
    [Header("Health Points")]
    [SerializeField]
    protected float MaxHP;

    [Header("Components")]
    [SerializeField]
    protected SpriteRenderer SpriteRenderer;
    [SerializeField]
    protected Animator Animator;

    [Header("Particle System prefab")]
    [SerializeField]
    protected GameObject DeathParticlesPrefab;

    protected float _hp;
    private int _triggeredTimes = 0;

    protected static Color s_damageColor = new(255, 0, 0);
    protected static Color s_defaultColor = new(255, 255, 255);

    protected const float DamageColorTime = 0.5f;
    protected const float DeathAnimationTime = 1f;

    public Waves Wave { get; set; }

    protected void OnEnable() => _hp = MaxHP;

    public virtual void DealDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
            StartCoroutine(Death());
        else
            StartCoroutine(DamageFrames());
    }

    protected IEnumerator DamageFrames()
    {
        Animator.SetTrigger("Hit");
        _triggeredTimes++;
        SpriteRenderer.color = s_damageColor;
        yield return new WaitForSeconds(DamageColorTime);

        _triggeredTimes--;
        if (_triggeredTimes == 0)
            SpriteRenderer.color = s_defaultColor;
    }

    public virtual IEnumerator Death()
    {
        SpriteRenderer.color = s_damageColor;
        yield return new WaitForSeconds(DeathAnimationTime);

        Destroy(gameObject);
        Wave.Killed++;

        GameObject particles = Instantiate(DeathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
