using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    [Header("Particle System prefab")]
    [SerializeField] GameObject HitParticlePrefab;
    [SerializeField] GameObject FallParticlePrefab;

    [field:Header("Stats")]
    [field:SerializeField]
    public Vector2 Direction { get; set; }
    [field:SerializeField]
    public float Speed { get; set; }
    [field:SerializeField]
    public float Range { get; set; }
    [field:SerializeField]
    public float Damage { get; set; }
    [field: SerializeField]
    public float KnockBack { get; set; }
    [field:SerializeField]
    public EnvironemtnState State { get; set; }
    public bool Record { get; set; } = false;
    public bool Rewind { get; set; } = false;

    private Vector3 _startingPos;
    private Stack<Vector3> _positions = new();
    private float _timeAfterDisable;
    private bool _disabled;

    private void Start()
    {
        _startingPos = transform.position;
        GetComponent<SpriteRenderer>().color = ColorByState();
        if (State == EnvironemtnState.Friendly)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        }
        else if (State == EnvironemtnState.Hostile)
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }

    private void FixedUpdate()
    {
        if (Rewind)
        {
            if (_disabled)
            {
                _timeAfterDisable -= Time.fixedDeltaTime;
                if (_timeAfterDisable <= 0)
                {
                    gameObject.GetComponent<CircleCollider2D>().enabled = true;
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    _disabled = false;
                }
            }
            if (_positions.Count > 0)
                transform.position = _positions.Pop();
            else
                Destroy(gameObject);
            return;
        }
        else if (Record)
        {
            _positions.Push(transform.position);
            if (_disabled)
                _timeAfterDisable += Time.fixedDeltaTime;
        }

        if (_disabled)
            return;

        if ((transform.position - _startingPos).magnitude >= Range)
        {
            if (Record)
            {
                DisableBullet();
                return;
            }
            DestroyBullet();
            return;
        }

        transform.Translate(Speed * Time.fixedDeltaTime * Direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Rewind)
            return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealthControle>().DealDamage(Damage);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Direction * KnockBack);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthControl>().DealDamage(Mathf.RoundToInt(Damage));
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Direction * KnockBack);
        }
        if (Record)
            DisableBullet();
        else
            DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
        GameObject particles = Instantiate(HitParticlePrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }

    private void DisableBullet()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _disabled = true;
        GameObject particles = Instantiate(HitParticlePrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }

    private Color ColorByState()
    {
        return State switch
        {
            EnvironemtnState.Friendly => Color.blue,
            EnvironemtnState.Hostile => Color.red,
            _ => Color.white
        };
    }
}
