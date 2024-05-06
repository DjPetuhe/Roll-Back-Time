using UnityEngine;

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

    private Vector3 _startingPos;

    private void Start()
    {
        _startingPos = transform.position;
        GetComponent<SpriteRenderer>().color = ColorByState();
    }

    private void FixedUpdate()
    {
        if ((transform.position - _startingPos).magnitude >= Range)
        {
            DestroyBullet();
            return;
        }

        transform.Translate(Speed * Time.fixedDeltaTime * Direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (State == EnvironemtnState.Hostile)
                return;

            other.gameObject.GetComponent<EnemyHealthControle>().DealDamage(Damage);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Direction * KnockBack);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (State == EnvironemtnState.Friendly)
                return;

            other.gameObject.GetComponent<PlayerHealthControl>().DealDamage(Mathf.RoundToInt(Damage));
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Direction * KnockBack);
        }
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
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
