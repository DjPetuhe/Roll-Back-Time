using UnityEngine;
using System.Collections;

public class EnemyHealthControle : MonoBehaviour
{
    [Header("Health Points")]
    [SerializeField] float MaxHP;

    [Header("Sprite Renderer")]
    [SerializeField] SpriteRenderer SpriteRenderer;

    [Header("Particle System prefab")]
    [SerializeField] GameObject DeathParticlesPrefab;

    private int _triggeredTimes = 0;
    private float _hp;

    private static Color s_damageColor = new(255, 0, 0);
    private static Color s_defaultColor = new(255, 255, 255);

    private const float DamageColorTime = 0.5f;
    private const float DeathAnimationTime = 1f;

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
        SpriteRenderer.color = s_damageColor;
        yield return new WaitForSeconds(DamageColorTime);

        _triggeredTimes--;
        if (_triggeredTimes == 0)
            SpriteRenderer.color = s_defaultColor;
    }

    public IEnumerator Death()
    {
        SpriteRenderer.color = s_damageColor;
        //GetComponent<PlayerMovement>().StopMovement(); stop ai movement
        yield return new WaitForSeconds(DeathAnimationTime);

        Destroy(gameObject);
        Wave.Killed++;

        GameObject particles = Instantiate(DeathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
