using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject BulletPrefab;

    [Header("Wand")]
    [SerializeField] GameObject Wand;
    [SerializeField] Transform Gunpoint;

    [Header("Joystick")]
    [SerializeField] Joystick Joystick;

    private float _timeLeft = 0;

    private GameManager _gameManager;
    private Vector2 _direction;

    public bool Rewind { get; set; } = false;

    private void OnEnable()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        _direction.y = Joystick.Vertical;
        _direction.x = Joystick.Horizontal;

        if (_direction == Vector2.zero || Rewind)
            Wand.SetActive(false);
        else
        {
            if (!Wand.activeSelf)
                Wand.SetActive(true);

            Wand.transform.rotation = Quaternion.LookRotation(Vector3.forward, -_direction);

            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                return;
            }

            _timeLeft = _gameManager.TimeBetweenShots;
            GameObject bullet = Instantiate(BulletPrefab, Gunpoint.transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Direction = _direction.normalized;
            bulletScript.Speed = _gameManager.BulletSpeed;
            bulletScript.Damage = _gameManager.Damage;
            bulletScript.Range = _gameManager.Range;
            bulletScript.KnockBack = 100; //temporary?
            bulletScript.State = EnvironemtnState.Friendly;
        }
    }
}