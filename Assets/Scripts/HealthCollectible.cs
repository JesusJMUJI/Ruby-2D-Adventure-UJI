    using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public ParticleSystem collectEffect;
    public AudioClip collectedClip;
    public BoxCollider2D collectibleCollider;
    void Start()
    {
        GameObject go = Instantiate(collectEffect.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
            if (controller.currentHealth < controller.maxHealth && other.CompareTag("Player"))
            {
                Despawn(controller);
            }
    }

    private void Despawn(RubyController controller)
    {
        collectibleCollider.enabled = false;
        collectEffect.Play();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        controller.ChangeHealth(1);
        Destroy(gameObject, 5);

        controller.PlaySound(collectedClip);
    }
}