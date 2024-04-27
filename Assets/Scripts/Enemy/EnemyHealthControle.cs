using UnityEngine;
using System.Collections;

public class EnemyHealthControle : MonoBehaviour
{
    [Header("Health Points")]
    [SerializeField] float MaxHP;

    [Header("Sprite Renderer")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Particle System prefab")]
    [SerializeField] GameObject deathParticlesPrefab;

    private int _triggeredTimes = 0;
    private float _hp;

    private static Color s_damageColor = new(255, 0, 0);
    private static Color s_defaultColor = new(255, 255, 255);

    private const float DAMAGE_COLOR_TIME = 0.5f;
    private const float DEATH_ANIMATION_TIME = 1f;

    public Waves Wave { get; set; }

    private void OnEnable() => _hp = MaxHP;

    public void DealDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
            StartCoroutine(Death());
        else
            StartCoroutine(DamageFrames());
    }

    private IEnumerator DamageFrames()
    {
        _triggeredTimes++;
        spriteRenderer.color = s_damageColor;
        yield return new WaitForSeconds(DAMAGE_COLOR_TIME);
        _triggeredTimes--;
        if (_triggeredTimes == 0)
            spriteRenderer.color = s_defaultColor;
    }

    public IEnumerator Death()
    {
        spriteRenderer.color = s_damageColor;
        //GetComponent<PlayerMovement>().StopMovement(); stop ai movement
        yield return new WaitForSeconds(DEATH_ANIMATION_TIME);
        Destroy(gameObject);
        Wave.Killed++;
        GameObject particles = Instantiate(deathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
