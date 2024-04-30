using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private LevelUI _levelUI;

    private float _currentTime = StartTimeSeconds;
    public float CurrentTime
    {
        get { return _currentTime; }
        set
        {
            if (value <= 0)
                _currentTime = 0;
            else 
                _currentTime = value;
            _levelUI.SetTime(_currentTime);
        }
    }

    private GameState _state;
    public GameState State
    {
        get { return _state; }
        set
        {
            Time.timeScale = value switch
            {
                GameState.Play => 1,
                _ => 0
            };
            _state = value;
        }
    }
    public int StateInt { get { return (int)_state; } }

    private int _curHealth = StartHealth;
    public int CurHealth
    {
        get { return _curHealth; }
        set
        {
            if (value <= 0)
                _curHealth = 0;
            else if (value >= Health)
                _curHealth = Health;
            else
                _curHealth = value;

            _levelUI.SetHealth(_curHealth, Health);
            if (_curHealth == 0)
                StartCoroutine(EndGame());
        }
    }

    private int _health = StartHealth;
    public int Health
    {
        get { return _health; }
        set
        {
            if (value <= MinHealth)
                _health = MinHealth;
            else if (value >= MaxHealth)
                _health = MaxHealth;
            else
            {

            }
                _health = value;

            _levelUI.SetHealth(CurHealth, _health);
        }
    }

    private float speed = StartSpeed;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value <= MinSpeed)
                speed = MinSpeed;
            else if (value >= MaxSpeed)
                speed = MaxSpeed;
            else speed = value;
        }
    }

    private float _timeBetweenShots = StartTimeBetweenShots;
    public float TimeBetweenShots
    {
        get { return _timeBetweenShots; }
        set
        {
            if (value <= MinTimeBetweenShots)
                _timeBetweenShots = MinTimeBetweenShots;
            else if (value >= MaxSpeed)
                _timeBetweenShots = MaxTimeBetweenShots;
            else
                _timeBetweenShots = value;
        }
    }

    private float _damage = StartDamage;
    public float Damage
    {
        get { return _damage; }
        set
        {
            if (value <= MinDamage)
                _damage = MinDamage;
            else if (value >= MaxDamage)
                _damage = MaxDamage;
            else
                _damage = value;
        }
    }

    private float _range = StartRange;
    public float Range
    {
        get { return _range; }
        set
        {
            if (value <= MinRange)
                _range = MinRange;
            else if (value >= MaxRange)
                _range = MaxRange;
            else
                _range = value;
        }
    }

    private float _bulletSpeed = StartBulletSpeed;
    public float BulletSpeed
    {
        get { return _bulletSpeed; }
        set
        {
            if (value <= MinBulletSpeed)
                _bulletSpeed = MinBulletSpeed;
            else if (value >= MaxBulletSpeed)
                _bulletSpeed = MaxBulletSpeed;
            else
                _bulletSpeed = value;
        }
    }

    private int _clearedWaves;
    public int ClearedWaves
    {
        get { return _clearedWaves; }
        set
        {
            if (value == _clearedWaves + 1)
                _clearedWaves++;
        }
    }

    private const float TimeBeforeGameOver = 2f;

    private const float StartTimeSeconds = 0;
    private const int StartHealth = 100;
    private const float StartSpeed = 4f;
    private const float StartTimeBetweenShots = 1;
    private const float StartDamage = 10;
    private const float StartRange = 5;
    private const float StartBulletSpeed = 3;

    public const int MinHealth = 10;
    public const int MaxHealth = 1000;
    public const float MinSpeed = 1;
    public const float MaxSpeed = 5;
    public const float MinTimeBetweenShots = 0.2f;
    public const float MaxTimeBetweenShots = 2f;
    public const float MinDamage = 1;
    public const float MaxDamage = 1000;
    public const float MinRange = 1;
    public const float MaxRange = 20;
    public const float MinBulletSpeed = 1;
    public const float MaxBulletSpeed = 10;

    private void Awake()
    {
        State = GameState.Play;
        _levelUI = GameObject.Find("LevelUI").GetComponent<LevelUI>();
    }

    /* Multiple levels not implemented
 
    private void OnEnable() => SceneManager.sceneLoaded += OnLevelLoading;

    private void OnDisable() => SceneManager.sceneLoaded -= OnLevelLoading;

    private void OnLevelLoading(Scene scene, LoadSceneMode mode) => AdaptGameManager();

    private void AdaptGameManager()
    {
        _levelUI = GameObject.Find("LevelUI").GetComponent<LevelUI>();
        if (LoadedGameState)
        {
            GameStateData gameState = SaveManager.LoadGameState();
            CurrentTime = gameState.Time;
            Health = gameState.Health;
            Speed = gameState.PlayerSpeed;
            BombsCount = gameState.BombAmount;
            ExplosionSize = gameState.ExplosionSize;
        }
        else CurrentTime = START_TIME_SECONDS;
        _levelUI.SetHealth(_health);
    }
    
     */

    private void Update()
    {
        if (State == GameState.Pause) return;
        if (CurrentTime > 0) CurrentTime -= 1 * Time.deltaTime;
    }

    private IEnumerator EndGame()
    {
        _levelUI.SwitchPauseStatus(false);
        yield return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthControl>().Death();
        yield return new WaitForSeconds(TimeBeforeGameOver);
        EndGameUI(true);
    }

    public void EndGameUI(bool gameOver)
    {
        State = GameState.GameEnd;
        _levelUI.EndGamePopUp(gameOver);
    }

    public void ResumeGame() => State = GameState.Play;

    public void PauseGame() => State = GameState.Pause;

    public void ApplyPerkChanges(PerkChanges perkChanges)
    {
        //TODO: Apply changes
    }
}