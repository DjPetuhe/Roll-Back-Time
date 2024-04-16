using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public float Range { get; set; }
    public float Damage { get; set; }
    public EnvironemtnState State { get; set; }

    private Vector3 _startingPos;

    private void OnEnable() => _startingPos = transform.position;

    private void FixedUpdate()
    {
        if ((transform.position - _startingPos).magnitude >= Range)
        {
            Destroy(gameObject);
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

            //Deal Damage to enemy
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (State == EnvironemtnState.Friendly)
                return;

            //Deal Damage to player
        }
        Destroy(gameObject);
    }
}
