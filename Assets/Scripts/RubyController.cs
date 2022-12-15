using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    #region Variables
    
    [Header("Health and related")]
    //
    public int maxHealth = 5;
    [SerializeField] private float timeInvincible = 2.0f;
    public int Health => currentHealth;

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

    private float maxSpeed = 5.0f;
    private float minSpeed = 3.0f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public bool canLaunch = true;
    
    [Header("Sound")]
    public AudioClip throwSound;
    public AudioClip damagedSound;
    public AudioSource movingSound;
    public AudioSource audioSource;

    [Header("UI")] 
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public Canvas controlsCanvas;
    Vector2 lookDirection = new Vector2(1, 0);
    Animator animator;

    public static RubyController instance;
    public int enemyCount;
    #endregion
    
    #region START, UPDATE Y FIXED UPDATE

    private void Awake()
    {
        if (RubyController.instance == null)
        {
            RubyController.instance = this;
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
         
        Application.targetFrameRate = 144;
         
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;

        StartCoroutine(ControlsPanel());
        IEnumerator ControlsPanel()
        {
            yield return new WaitForSeconds(5);
            controlsCanvas.enabled = false;
        }
    }
 
     // Update is called once per frame
    void Update()
    {
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Resetting scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.LeftShift) && speed <= maxSpeed)
        {
            print("Sprinting");
            speed += 4f * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = minSpeed;
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

    public void FixedEnemy()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            StartCoroutine(WinCoroutine());
        }
    }
    
    public void ChangeHealth(int amount)
    {
        if (currentHealth == 0)
        {
            StartCoroutine(LoseCoroutine());
        }
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
        if (canLaunch)
        {
            canLaunch = false;
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f,
                Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
            animator.SetTrigger("Launch");
            audioSource.PlayOneShot(throwSound);
            StartCoroutine(ProjectileCooldownCoroutine());
        }
    }
    
    IEnumerator ProjectileCooldownCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        canLaunch = true;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("The scene has been reset");
    }
    
    public void ExitGame()
    {
        Application.Quit(); 
        Debug.Log("The game has been closed");
    }
    
    #endregion

    #region Corrutinas
    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1);
        winPanel.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    IEnumerator LoseCoroutine()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        AudioListener.pause = true;
        yield return new WaitForSeconds(1);
    }
    

    #endregion
}