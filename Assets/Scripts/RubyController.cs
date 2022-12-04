using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RubyController : MonoBehaviour
{
    #region Variables
    
    [Header("Health and related")]
    //
    public int maxHealth = 5;
    [SerializeField] private float timeInvincible = 2.0f;
    public int health { get { return currentHealth; } }
    [Range(0,10)]
    public int currentHealth;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float invincibleTimer;
    //
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private float horizontal; 
    [SerializeField] private float vertical;    
    [Range(3f, 7f)]
    public float speed = 4.0f;

    private float maxSpeed = 7.0f;
    private float minSpeed = 3.0f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    
    [Header("Sound")]
    public AudioClip throwSound;
    public AudioClip damagedSound;
    public AudioSource movingSound;
    public AudioSource audioSource;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    
    #endregion
    
    #region START, UPDATE Y FIXED UPDATE
    void Start()
    {
         animator = GetComponent<Animator>();
         rigidbody2d = GetComponent<Rigidbody2D>();
         currentHealth = maxHealth;

         Application.targetFrameRate = 165;
    }
 
     // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
        {
            if (!movingSound.isPlaying)
            {
                movingSound.Play();
            }
        }
        else
        {
            if (movingSound.isPlaying)
            {
                movingSound.Stop();
            }
        }
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && speed <= maxSpeed)
        {
            print("Sprinting"); ;
            speed += 4f * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = minSpeed;
        }

        Vector2 move = new Vector2(horizontal, vertical);
         
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
         
         
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
         
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, 
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialogue();
                }
            }
        }
    }
 
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
 
        rigidbody2d.MovePosition(position);
    }
    #endregion

    #region Metodos

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {

            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(damagedSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, 
            rigidbody2d.position + Vector2.up * 0.5f, 
            Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>(); 
        projectile.Launch(lookDirection, 300);
        
        audioSource.PlayOneShot(throwSound);
        animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    #endregion
}