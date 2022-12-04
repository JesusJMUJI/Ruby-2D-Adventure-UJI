using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [Header("Movement")]
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public new Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    
    [Header("Particles")]
    public ParticleSystem smokeEffect;
    public ParticleSystem hitEffect;
    bool broken = true;
    Animator animator;
    
    [Header("Audio")]
    public AudioClip fixSound;
    
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            
            return;
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
        
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            
            return;
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        hitEffect.Play();
        smokeEffect.Stop();
        AudioSource.PlayClipAtPoint(fixSound, transform.position);
        _audioSource.Stop();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    
}
