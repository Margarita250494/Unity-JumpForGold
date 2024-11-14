using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private float speed = 6f; 
    private int lives = 5; 
    public Image[] hearts;
    private float jumpForce = 15f; 
    public Text goldText;
    public int gold = 0; 
    public GameObject loseScene;
    public GameObject wonScene;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded = false;
    private Animator anim;

    public AudioSource jumpAudio, coinsAudio, lifeAudio, loseAudio, wonAudio;

    public static Hero Instance { get; set; }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        UpdateSpriteDirection(horizontalInput);

        if (isGrounded)
        {
            State = horizontalInput != 0 ? States.run : States.idle;
        }

        if (isGrounded && Input.GetButtonDown("Jump")) Jump();

        if (transform.position.y < -200) GameOver();

    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void UpdateSpriteDirection(float horizontalInput)
    {
        if (horizontalInput != 0) sprite.flipX = horizontalInput < 0;
    }

    private void Jump()
    {
        jumpAudio.Play();
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        State = States.jump;
    }


    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.down * 0.5f, 0.3f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = States.jump;
    }

    public void GetDamage()
    {
        lifeAudio.Play();
        lives -= 1;
        UpdateHearts();

        if (lives <= 0) GameOver();
    }
    public void AddGold(int amount)
    {
        coinsAudio.Play();
        gold += amount;
        UpdateGoldUI();
    }
    public void GameOver()
    {
        loseAudio.Play();
        loseScene.SetActive(true);
        Destroy(gameObject);
    }
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < lives);
        }
    }
    private void UpdateGoldUI()
    {
        if (goldText != null) goldText.text = gold.ToString();
    }
}

public enum States
{
    idle,
    run,
    jump
}
