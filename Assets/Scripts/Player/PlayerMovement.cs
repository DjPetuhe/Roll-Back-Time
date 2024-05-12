using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player animator")]
    [SerializeField] Animator Animator;

    [Header("Player rigidbody")]
    [SerializeField] Rigidbody2D Rb2d;

    [Header("Joystick")]
    [SerializeField] Joystick Joystick;

    private GameManager _gameManager;
    private PlayerHealthControl _playerHealthControl;

    private int _dir = 1;
    public int Dir
    {
        get { return _dir; }
        private set
        {
            _dir = value;
            transform.localScale = _dir == 4 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }

    private Vector2 _direction;

    private const float Epsilon = 0.001f;

    public bool Record { get; set; } = false;
    public bool Rewind { get; set; } = false;
    private Stack<Vector3> _positions = new();
    private Stack<Vector2> _directions = new();

    private void OnEnable()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _playerHealthControl = GetComponent<PlayerHealthControl>();
    }

    private int FindDirection()
    {
        if (_direction.sqrMagnitude > Epsilon)
        {
            if (_direction.x > _direction.y)
                Dir = (_direction.x > -_direction.y) ? 2 : 1;
            else
                Dir = (_direction.x < -_direction.y) ? 4 : 3;
        }
        return Dir;
    }

    private void Update()
    {
        if (_playerHealthControl.State == PlayerState.Dead)
            return;
        if (_gameManager.State == GameState.Pause || _gameManager.State == GameState.GameEnd)
            return;
        if (Rewind)
            return;
        _direction.x = Joystick.Horizontal;
        _direction.y = Joystick.Vertical;
        _gameManager.SkillTimeScale = _direction.magnitude * 0.9f + 0.1f;
        ConfigureAnimator();
    }

    public void ConfigureAnimator()
    {
        Animator.SetFloat("Horizontal", _direction.x);
        Animator.SetFloat("Vertical", _direction.y);
        Animator.SetFloat("Speed", _direction.magnitude);
        Animator.SetInteger("Direction", FindDirection());
    }

    public void StopMovement()
    {
        _direction = new(0, 0);
        ConfigureAnimator();
    }

    private void FixedUpdate()
    {
        if (Record)
        {
            _positions.Push(transform.position);
            _directions.Push(_direction);
        }
        if (Rewind)
        {
            if (_positions.Count > 0)
                transform.position = _positions.Pop();
            if (_directions.Count > 0)
            {
                _direction = _directions.Pop();
                ConfigureAnimator();
            }
            return;
        }
        Rb2d.MovePosition(Rb2d.position + _gameManager.Speed * Time.fixedDeltaTime * _direction);
    }

    public void SpeedUpBy(float speedUp) => _gameManager.Speed += speedUp;
}