using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BossAI : MonoBehaviour
{
    [Header("Attack stats")]
    [SerializeField] int attack1Bullets;
    [SerializeField] int attack2Bullets;
    [SerializeField] int attack3Bullets;
    [SerializeField] int attack4Bullets;

    [SerializeField] float attack1TimeBetweenWaves;
    [SerializeField] float attack2TimeBetweenWaves;
    [SerializeField] float attack3TimeBetweenWaves;
    [SerializeField] float attack4TimeBetweenWaves;

    [SerializeField] int attack1Waves;
    [SerializeField] int attack2Waves;
    [SerializeField] int attack3Waves;
    [SerializeField] int attack4Waves;

    [Header("Stats Circle")]
    [SerializeField] float Damage;
    [SerializeField] float Range;
    [SerializeField] float BulletSpeed;

    [Header("Bullet")]
    [SerializeField] GameObject BulletPrefab;

    public bool Active { get; private set; }

    private readonly List<Func<IEnumerator>> _attacks = new();
    private readonly WaitForSeconds _timeBetweenAttacks = new(5);
    private GameObject _player;
    private PlayerMovement _playerMovement;
    private PlayerShooting _playerShooting;
    private LevelUI _ui;
    private GameManager _gameManager;
    private readonly List<Bullet> _recordedBullets = new();
    private float _recordStartTime;

    public void Awake()
    {
        _attacks.Add(Attack1);
        _attacks.Add(Attack2);
        _attacks.Add(Attack3);
        _player = GameObject.FindGameObjectWithTag("Player");
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _ui = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<LevelUI>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _playerShooting = _player.GetComponent<PlayerShooting>();
    }

    public void Activate()
    {
        Active = true;
        NextAttack(-1);
    }

    public void NextAttack(int previousAttack)
    {
        int index;
        do
        {
            index = Random.Range(0, _attacks.Count);
        } while (index == previousAttack);
        StartCoroutine(_attacks[index].Invoke());
    }

    public IEnumerator Attack1()
    {
        float initialAngle = 0f;
        float rotationAngle = 360f / attack1Waves;
        for (int wave = 0; wave < attack1Waves; wave++)
        {
            float angleStep = 360f / attack1Bullets;
            Vector3 startPos = new(1, 0, 0);
            float currentAngle = wave % 2 == 0 ? initialAngle : initialAngle +  rotationAngle / 2;
            for (int i = 0; i < attack1Bullets; i++)
            {
                float angle = i * angleStep + currentAngle;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * startPos;
                Vector3 pos = transform.position + dir;

                GameObject bullet = Instantiate(BulletPrefab, pos, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                AdjustBullet(bulletScript, dir);
            }
            yield return new WaitForSeconds(attack1TimeBetweenWaves);
        }
        yield return _timeBetweenAttacks;
        NextAttack(0);
    }


    public IEnumerator Attack2()
    {
        Record(true);
        int _hpBefore = _gameManager.CurHealth;
        yield return Attack2Trajectory(true);
        yield return new WaitForSeconds(2);
        Record(false);
        if (_hpBefore == _gameManager.CurHealth)
        {
            yield return Rewind();
            yield return Attack2Trajectory(false);
        }
        yield return _timeBetweenAttacks;
        NextAttack(1);
    }

    private IEnumerator Attack2Trajectory(bool record)
    {
        for (int wave = 0; wave < attack2Waves; wave++)
        {
            if (_player == null)
            {
                StopAllCoroutines();
                break;
            }
            Vector3 dir = (_player.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            AdjustBullet(bulletScript, dir, record);
            if (record)
                _recordedBullets.Add(bulletScript);
            yield return new WaitForSeconds(attack2TimeBetweenWaves);
        }
    }

    public IEnumerator Attack3()
    {
        float rotationAngle = 360f / attack3Bullets;
        float waveAngle = 90f / attack3Waves;
        float currentAngle = 0;
        for (int wave = 0; wave < attack3Waves; wave++)
        {
            Vector3 startPos = new(1, 0, 0);
            currentAngle += waveAngle;
            for (int i = 0; i < attack3Bullets; i++)
            {
                float angle = i * rotationAngle + currentAngle;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * startPos;
                Vector3 pos = transform.position + dir;

                GameObject bullet = Instantiate(BulletPrefab, pos, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                AdjustBullet(bulletScript, dir);
            }
            yield return new WaitForSeconds(attack3TimeBetweenWaves);
        }
        yield return _timeBetweenAttacks;
        NextAttack(2);
    }

    private void AdjustBullet(Bullet bulletScript, Vector3 dir, bool record = false)
    {
        bulletScript.Direction = dir.normalized;
        bulletScript.Speed = BulletSpeed;
        bulletScript.Damage = Damage;
        bulletScript.Range = Range;
        bulletScript.KnockBack = 100;
        bulletScript.State = EnvironemtnState.Hostile;
        bulletScript.Record = record;
    }

    public void Record(bool active)
    {
        if (active)
            _recordStartTime = Time.time;
        _gameManager.Record = active;
        _playerMovement.Record = active;
        if (!active)
        {
            foreach (var bullet in _recordedBullets)
                bullet.Record = false;
        }
    }

    public IEnumerator Rewind()
    {
        _gameManager.Rewind = true;
        _playerMovement.Rewind = true;
        _playerShooting.Rewind = true;
        _ui.RewindPanelActivation(true);
        foreach (var bullet in _recordedBullets)
            bullet.Rewind = true;

        yield return new WaitForSeconds(Time.time - _recordStartTime);

        _gameManager.Rewind = false;
        _playerMovement.Rewind = false;
        _playerShooting.Rewind = false;
        _ui.RewindPanelActivation(false);
        foreach (var bullet in _recordedBullets)
            bullet.Rewind = false;
        _recordedBullets.Clear();
    }
}
