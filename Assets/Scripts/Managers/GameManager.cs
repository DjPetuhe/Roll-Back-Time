using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private LevelUI _levelUI;

    private float _currentTime = START_TIME_SECONDS;
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

    private int _curHealth = START_HEALTH;
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

    private int _health = START_HEALTH;
    public int Health
    {
        get { return _health; }
        set
        {
            if (value <= MIN_HEALTH)
                _health = MIN_HEALTH;
            else if (value >= MAX_HEALTH)
                _health = MAX_HEALTH;
            else
            {

            }
                _health = value;

            _levelUI.SetHealth(CurHealth, _health);
        }
    }

    private float speed = START_SPEED;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value <= MIN_SPEED)
                speed = MIN_SPEED;
            else if (value >= MAX_SPEED)
                speed = MAX_SPEED;
            else speed = value;
        }
    }

    private float _timeBetweenShots = START_TIME_BETWEEN_SHOTS;
    public float TimeBetweenShots
    {
        get { return _timeBetweenShots; }
        set
        {
            if (value <= MIN_TIME_BETWEEN_SHOTS)
                _timeBetweenShots = MIN_TIME_BETWEEN_SHOTS;
            else if (value >= MAX_SPEED)
                _timeBetweenShots = MAX_TIME_BETWEEN_SHOTS;
            else
                _timeBetweenShots = value;
        }
    }

    private float _damage = START_DAMAGE;
    public float Damage
    {
        get { return _damage; }
        set
        {
            if (value <= MIN_DAMAGE)
                _damage = MIN_DAMAGE;
            else if (value >= MAX_DAMAGE)
                _damage = MAX_DAMAGE;
            else
                _damage = value;
        }
    }

    private float _range = START_RANGE;
    public float Range
    {
        get { return _range; }
        set
        {
            if (value <= MIN_RANGE)
                _range = MIN_RANGE;
            else if (value >= MAX_RANGE)
                _range = MAX_RANGE;
            else
                _range = value;
        }
    }

    private float _bulletSpeed = START_BULLET_SPEED;
    public float BulletSpeed
    {
        get { return _bulletSpeed; }
        set
        {
            if (value <= MIN_BULLET_SPEED)
                _bulletSpeed = MIN_BULLET_SPEED;
            else if (value >= MAX_BULLET_SPEED)
                _bulletSpeed = MAX_BULLET_SPEED;
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

    private const float TIME_BEFORE_GAME_OVER = 2f;

    private const float START_TIME_SECONDS = 0;
    private const int START_HEALTH = 100;
    private const float START_SPEED = 4f;
    private const float START_TIME_BETWEEN_SHOTS = 1;
    private const float START_DAMAGE = 10;
    private const float START_RANGE = 5;
    private const float START_BULLET_SPEED = 3;

    public const int MIN_HEALTH = 10;
    public const int MAX_HEALTH = 1000;
    public const float MIN_SPEED = 1;
    public const float MAX_SPEED = 5;
    public const float MIN_TIME_BETWEEN_SHOTS = 0.2f;
    public const float MAX_TIME_BETWEEN_SHOTS = 2f;
    public const float MIN_DAMAGE = 1;
    public const float MAX_DAMAGE = 1000;
    public const float MIN_RANGE = 1;
    public const float MAX_RANGE = 20;
    public const float MIN_BULLET_SPEED = 1;
    public const float MAX_BULLET_SPEED = 10;

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
        yield return new WaitForSeconds(TIME_BEFORE_GAME_OVER);
        EndGameUI(true);
    }

    public void EndGameUI(bool gameOver)
    {
        State = GameState.GameEnd;
        _levelUI.EndGamePopUp(gameOver);
    }

    public void ResumeGame() => State = GameState.Play;

    public void PauseGame() => State = GameState.Pause;
}