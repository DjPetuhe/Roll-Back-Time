using UnityEngine;
using System.Collections;

public class PlayerHealthControl : MonoBehaviour
{

    [Header("Sprite Renderer")]
    [SerializeField] SpriteRenderer SpriteRenderer;

    [Header("Particle System prefab")]
    [SerializeField] GameObject DeathParticlesPrefab;

    private GameManager _gameManager;

    private static Color s_damageColor = new(255, 0, 0);
    private static Color s_defaultColor = new(255, 255, 255);

    private const int BlinkingAmount = 3;
    private const float BlinkingTime = 0.25f;
    private const float BlinkingGapTime = 0.25f;
    private const float DeathAnimationTime = 1f;

    public PlayerState State { get; private set; } = PlayerState.Normal;

    private void OnEnable() => _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    public void DealDamage(int damage)
    {
        if (_gameManager.CurHealth <= 0 || _gameManager.State == GameState.GameEnd)
            return;

        _gameManager.CurHealth -= Mathf.RoundToInt(damage * _gameManager.IncomingDamageMultiplyer);
        StartCoroutine(InvincibleFrames());
    }

    private IEnumerator InvincibleFrames()
    {
        State = PlayerState.Invincible;
        for (int i = 0; i < BlinkingAmount; i++)
        {
            SpriteRenderer.color = s_damageColor;
            yield return new WaitForSeconds(BlinkingTime);
            SpriteRenderer.color = s_defaultColor;
            if (i != BlinkingAmount - 1)
                yield return new WaitForSeconds(BlinkingGapTime);
        }
        State = PlayerState.Normal;
    }

    public IEnumerator Death()
    {
        State = PlayerState.Dead;
        SpriteRenderer.color = s_damageColor;
        GetComponent<PlayerMovement>().StopMovement();
        yield return new WaitForSeconds(DeathAnimationTime);
        GameObject particles = Instantiate(DeathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }

    public void RestoreHealth(int heal) => _gameManager.CurHealth += heal;
}