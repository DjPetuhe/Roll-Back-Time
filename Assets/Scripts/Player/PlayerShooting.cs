using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;

    [Header("Wand")]
    [SerializeField] GameObject wand;
    [SerializeField] Transform gunpoint;

    [Header("Joystick")]
    [SerializeField] Joystick joystick;

    private float _timeLeft = 0;

    private GameManager _gameManager;
    private Vector2 _direction;

    private void OnEnable()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        _direction.y = joystick.Vertical;
        _direction.x = joystick.Horizontal;

        if (_direction == Vector2.zero)
            wand.SetActive(false);
        else
        {
            if (!wand.activeSelf)
                wand.SetActive(true);

            wand.transform.rotation = Quaternion.LookRotation(Vector3.forward, -_direction);

            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                return;
            }

            _timeLeft = _gameManager.TimeBetweenShots;
            GameObject bullet = Instantiate(bulletPrefab, gunpoint.transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Direction = _direction.normalized;
            bulletScript.Speed = _gameManager.BulletSpeed;
            bulletScript.Damage = _gameManager.Damage;
            bulletScript.Range = _gameManager.Range;
            bulletScript.State = EnvironemtnState.Friendly;
        }
    }
}