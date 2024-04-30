using UnityEngine;

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

    private (int, int) _cellPosition;
    public (int, int) CellPosition
    {
        get { return _cellPosition; }
        private set
        {
            _cellPosition = value;
        }
    }

    private Vector2 _direction;

    private const float Epsilon = 0.001f;

    private void OnEnable()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _playerHealthControl = GetComponent<PlayerHealthControl>();
        CellPosition = (Mathf.RoundToInt(Rb2d.position.y), Mathf.RoundToInt(Rb2d.position.x));
    }

    private int FindDirection()
    {
        if (_direction.sqrMagnitude > Epsilon)
        {
            if (_direction.x > _direction.y) Dir = (_direction.x > -_direction.y) ? 2 : 1;
            else Dir = (_direction.x < -_direction.y) ? 4 : 3;
        }
        return Dir;
    }

    private void Update()
    {
        if (_playerHealthControl.State == PlayerState.Dead) return;
        if (_gameManager.State == GameState.Pause || _gameManager.State == GameState.GameEnd) return;
        _direction.x = Joystick.Horizontal;
        _direction.y = Joystick.Vertical;
        ConfigureAnimator();
    }

    public void ConfigureAnimator()
    {
        /* Not yet animated
        animator.SetFloat("Horizontal", _direction.x);
        animator.SetFloat("Vertical", _direction.y);
        animator.SetFloat("Speed", _direction.magnitude);
        animator.SetInteger("Direction", FindDirection());
        */
    }

    public void StopMovement()
    {
        _direction = new(0, 0);
        ConfigureAnimator();
    }

    private void FixedUpdate()
    {
        Rb2d.MovePosition(Rb2d.position + _gameManager.Speed * Time.fixedDeltaTime * _direction);
        (int, int) currentPos = (Mathf.RoundToInt(Rb2d.position.y), Mathf.RoundToInt(Rb2d.position.x));
        if (currentPos != CellPosition) CellPosition = currentPos;
    }

    public void SpeedUpBy(float speedUp) => _gameManager.Speed += speedUp;
}