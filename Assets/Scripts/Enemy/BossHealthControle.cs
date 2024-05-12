using UnityEngine;
using System.Collections;

public class BossHealthControle : EnemyHealthControle 
{
    [Header("BossAI")]
    [SerializeField] BossAI AI;

    private GameManager _gameManager;

    private LevelUI _ui;
    private void Awake()
    {
        _ui = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<LevelUI>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public override void DealDamage(float damage)
    {
        if (_gameManager.State == GameState.GameEnd)
            return;

        _hp -= damage;
        _ui.SetBossHealth(_hp, MaxHP);
        if (_hp <= 0)
            StartCoroutine(Death());
        else
            StartCoroutine(DamageFrames());
    }

    public override IEnumerator Death()
    {
        SpriteRenderer.color = s_damageColor;
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _gameManager.StartCoroutine(_gameManager.EndGame(false));

        yield return new WaitForSeconds(DeathAnimationTime);

        Destroy(gameObject);

        GameObject particles = Instantiate(DeathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
        particles.transform.localScale = new(3, 3, 1);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
