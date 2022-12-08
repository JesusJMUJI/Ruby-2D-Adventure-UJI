using UnityEngine;

public class Projectile : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
    
    public void Launch(Vector2 direction, float force = 5)
    {
        rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }
    }
}
