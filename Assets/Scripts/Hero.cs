using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private float speed = 6f;
    private int initialLives = 5;
    private float jumpForce = 15f;

    [SerializeField] private Image[] heartImages;
    [SerializeField] private Text goldTextUI;

    [SerializeField] private GameObject loseScene;
    public GameObject wonScene;

    private bool isGrounded = false;

    public int gold = 0;
    public static Hero Instance { get; set; }
    private HeroStates State
    {
        get { return (HeroStates)anim.GetInteger("state"); }
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
        HandleInput();
        if (transform.position.y < -200) GameOver();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        UpdateSpriteDirection(horizontalInput);

        if (isGrounded) State = horizontalInput != 0 ? HeroStates.run : HeroStates.idle;

        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
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
        AudioController.Instance.PlayJumpAudio();
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        State = HeroStates.jump;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.down * 0.5f, 0.3f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = HeroStates.jump;
    }

    public void GetDamage()
    {
        AudioController.Instance.PlayLifeAudio();
        initialLives -= 1;
        UpdateHearts();

        if (initialLives <= 0) GameOver();
    }
    public void AddGold(int amount)
    {
        AudioController.Instance.PlayCoinAudio();
        gold += amount;
        UpdateGoldUI();
    }
    public void GameOver()
    {
        AudioController.Instance.PlayLoseAudio();
        loseScene.SetActive(true);
        Destroy(gameObject);
    }
    private void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = (i < initialLives);
        }
    }
    private void UpdateGoldUI()
    {
        if (goldTextUI != null) goldTextUI.text = gold.ToString();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Определяем угол столкновения
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            // Если столкновение произошло сбоку платформы
            if (Mathf.Abs(normal.x) > 0.5f && Mathf.Abs(normal.y) < 0.5f)
            {
                // Добавляем небольшой толчок от платформы
                rb.AddForce(new Vector2(normal.x * 5f, 0), ForceMode2D.Impulse);
            }

            // Если герой приземлился сверху на платформу
            if (normal.y > 0.5f)
            {
                isGrounded = true;
            }
        }
    }
}

public enum HeroStates
{
    idle,
    run,
    jump
}
