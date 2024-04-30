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
    [field:SerializeField]
    public EnvironemtnState State { get; set; }

    private Vector3 _startingPos;

    private void OnEnable() => _startingPos = transform.position;

    private void FixedUpdate()
    {
        if ((transform.position - _startingPos).magnitude >= Range)
        {
            Destroy(gameObject);
            GameObject particles = Instantiate(FallParticlePrefab, gameObject.transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().Play();
            return;
        }

        transform.Translate(Speed * Time.fixedDeltaTime * Direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (State == EnvironemtnState.hostile)
                return;

            other.gameObject.GetComponent<EnemyHealthControle>().DealDamage(Damage);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (State == EnvironemtnState.Friendly)
                return;

            //Deal Damage to player
        }
        Destroy(gameObject);
        GameObject particles = Instantiate(HitParticlePrefab, gameObject.transform.position, Quaternion.identity);
        particles.GetComponent<ParticleSystem>().Play();
    }
}
