using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private LevelUI _levelUI;

    [SerializeField]
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

    [SerializeField]
    private GameState _state;
    public GameState State
    {
        get { return _state; }
        set
        {
            Time.timeScale = value switch
            {
                GameState.Play => SkillTimeScale,
                _ => 0
            };
            _state = value;
        }
    }
    public int StateInt { get { return (int)_state; } }

    [SerializeField]
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

    [SerializeField]
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
                _health = value;

            if (CurHealth > _health)
                CurHealth = _health;

            _levelUI.SetHealth(CurHealth, _health);
        }
    }

    [SerializeField]
    private float _speed = StartSpeed;
    public float Speed
    {
        get { return _speed; }
        set
        {
            if (value <= MinSpeed)
                _speed = MinSpeed;
            else if (value >= MaxSpeed)
                _speed = MaxSpeed;
            else _speed = value;
        }
    }

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
    private float _incomingDamageMultiplyer = StartIncDamageMultiplyer;
    public float IncomingDamageMultiplyer
    {
        get { return _incomingDamageMultiplyer; }
        set
        {
            if (value <= MinIncDamageMultiplyer)
                _incomingDamageMultiplyer = MinIncDamageMultiplyer;
            else if (value >= MaxIncDamageMultiplyer)
                _incomingDamageMultiplyer = MaxIncDamageMultiplyer;
            else
                _incomingDamageMultiplyer = value;
        }
    }

    [SerializeField]
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

    private float _skillTimeScale;
    public float SkillTimeScale
    {
        get
        {
            if (SkillActive)
                return _skillTimeScale;
            return 1;
        }
        set
        {
            if (value < 0)
                _skillTimeScale = 0;
            else if (value > 1)
                _skillTimeScale = 1;
            else
                _skillTimeScale = value;
        }
    }

    private float _skillCooldown;
    public float SkillCooldown
    {
        get { return _skillCooldown; }
        set
        {
            if (value < 0)
            {
                _skillCooldown = 0;
                _levelUI.DeactivateCooldownImage();
            }
            else if (value > CooldownTime)
                _skillCooldown = CooldownTime;
            else
            {
                _skillCooldown = value;
                _levelUI.FillCooldownImage(_skillCooldown / CooldownTime);
            }
        }
    }

    private float _skillTimePassed;
    public float SkillTimePassed
    {
        get { return _skillTimePassed; }
        private set
        {
            if (value < 0)
                _skillTimePassed = 0;
            else if (value > SkillTime)
            {
                _skillTimePassed = SkillTime;
                SkillActive = false;
                _levelUI.DeactivateSkillTimeImage();
            }
            else
            {
                _skillTimePassed = value;
                _levelUI.FillSkillTimeImage(_skillTimePassed / SkillTime);
            }
        }
    }

    private bool _skillActive;
    public bool SkillActive
    {
        get { return _skillActive; }
        set
        {
            if (value == _skillActive)
                return;
            if (value && _skillCooldown == 0)
            {
                SkillTimePassed = 0;
                _skillActive = value;
            }
            else if (!value && SkillTimePassed >= SkillTime)
            {
                _skillActive = value;
                SkillCooldown = CooldownTime;
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }
    }

    private const float TimeBeforeGameOver = 2f;

    private const float StartTimeSeconds = 0;
    private const int StartHealth = 100;
    private const float StartSpeed = 2f;
    private const float StartTimeBetweenShots = 1;
    private const float StartDamage = 10;
    private const float StartRange = 5;
    private const float StartBulletSpeed = 3;
    private const float StartIncDamageMultiplyer = 1;

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
    public const float MinIncDamageMultiplyer = 0.1f;
    public const float MaxIncDamageMultiplyer = 5f;
    public const float CooldownTime = 60;
    public const float SkillTime = 10;

    private void Awake()
    {
        State = GameState.Play;
        _levelUI = GameObject.Find("LevelUI").GetComponent<LevelUI>();
    }

    private void Update()
    {
        if (State == GameState.Pause)
            return;
        if (CurrentTime > 0)
            CurrentTime -= 1 * Time.deltaTime;
        if (SkillActive)
        {
            SkillTimePassed += Time.deltaTime * Mathf.Pow(SkillTimeScale, -1);
            Time.timeScale = SkillTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else if (SkillCooldown > 0)
            SkillCooldown -= Time.deltaTime;

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
        Health += (int)perkChanges.MaxHealthChange;
        Health = (int)Mathf.Ceil(Health * perkChanges.MaxHealthMultiplyer);
        CurHealth += (int)perkChanges.HealthChange;
        CurHealth = (int)Mathf.Ceil(CurHealth * perkChanges.HealthMultiplyer);
        Damage += perkChanges.DamageChange;
        Damage *= perkChanges.DamageMultiplyer;
        Range += perkChanges.RangeChange;
        Range *= perkChanges.RangeMultiplyer;
        TimeBetweenShots += perkChanges.TimeBetweenShotsChange;
        TimeBetweenShots *= perkChanges.TimeBetweenShotsMultiplyer;
        Speed += perkChanges.SpeedChange;
        Speed *= perkChanges.SpeedMultiplyer;
        BulletSpeed += perkChanges.BulletSpeedChange;
        BulletSpeed *= perkChanges.BulletSpeedMultiplyer;
        IncomingDamageMultiplyer *= perkChanges.IncomingDamageMultiplyer;
    }
}