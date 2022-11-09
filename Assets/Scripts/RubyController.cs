using UnityEngine;

public class RubyController : MonoBehaviour
{ 
    public int maxHealth = 5; 
    public int currentHealth;
    
    private Rigidbody2D rigidbody2d;
    private float horizontal; 
    private float vertical;
    public float speed = 3.0f;
    
     // Start is called before the first frame update
     void Start()
     {
         rigidbody2d = GetComponent<Rigidbody2D>();
         currentHealth = maxHealth;
         currentHealth = 1;
     }
 
     // Update is called once per frame
     void Update()
     {
         horizontal = Input.GetAxis("Horizontal");
         vertical = Input.GetAxis("Vertical");
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
         currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
         Debug.Log(currentHealth + "/" + maxHealth);
     }
 }