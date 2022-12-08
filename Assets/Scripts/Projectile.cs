using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public bool hasCollided = false;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f || !hasCollided)
        {
            Destroy(gameObject, 2f);
        }
    }
    
    public void Launch(Vector2 direction, float force = 5)
    {
        rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        Debug.Log("Projectile collided with " + other.gameObject.name); 
        EnemyController e = other.collider.GetComponent<EnemyController>(); 
        if (e != null && e.CompareTag("Enemy")) 
        {
            e.Fix();
        }

        StartCoroutine(Collide());
    }

    IEnumerator Collide()
    {
        yield return new WaitForSeconds(1);
        rigidbody2D.simulated = false;
        Destroy(gameObject);
    }
}
