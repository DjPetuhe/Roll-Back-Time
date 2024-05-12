using UnityEngine;

public class BossActivation : MonoBehaviour
{
    [SerializeField] BossAI BossAI;
    
    private GameManager _gameManager;
    private LevelUI _ui;

    private void Awake()
    {
        _ui = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<LevelUI>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (BossAI.Active)
            return;

        BossAI.Activate();
        _ui.ActivateBossHealth();
        _ui.DisableSkill();
        _gameManager.DeactivateSkill();
    }
}
