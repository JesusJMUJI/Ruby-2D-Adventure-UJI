using UnityEngine;

public class RubyController : MonoBehaviour
{ 
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    private Rigidbody2D rigidbody2d;
    private float horizontal; 
    private float vertical;
    public float speed = 3.0f;
    
    public int health { get { return currentHealth; } }
    public int currentHealth;
    
    public bool isInvincible;
    public float invincibleTimer;
    
    public GameObject projectilePrefab;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
     // Start is called before the first frame update
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
         horizontal = Input.GetAxis("Horizontal");
         vertical = Input.GetAxis("Vertical");

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
     }
 
     void FixedUpdate()
     {
         Vector2 position = rigidbody2d.position;
         position.x = position.x + speed * horizontal * Time.deltaTime;
         position.y = position.y + speed * vertical * Time.deltaTime;
 
         rigidbody2d.MovePosition(position);
     }

     public void ChangeHealth(int amount)
     {
         if (amount < 0)
         {
             if (isInvincible)
                 return;
             isInvincible = true;
             invincibleTimer = timeInvincible;
         }
         animator.SetTrigger("Hit");
         currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
         Debug.Log(currentHealth + "/" + maxHealth);
     }

     void Launch()
     {
         GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

         Projectile projectile = projectileObject.GetComponent<Projectile>(); 
         projectile.Launch(lookDirection, 300);
         
         animator.SetTrigger("Launch");
     }
 }