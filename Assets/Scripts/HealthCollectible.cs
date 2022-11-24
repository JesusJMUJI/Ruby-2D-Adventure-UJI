    using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public ParticleSystem collectEffect;
    void Start()
    {
        GameObject go = Instantiate(collectEffect.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller.currentHealth < controller.maxHealth)
        {
            Despawn(controller);
        }
    }

    private void Despawn(RubyController controller)
    {
        collectEffect.Play();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        controller.ChangeHealth(1);
        Destroy(gameObject, 2);
    }
}
