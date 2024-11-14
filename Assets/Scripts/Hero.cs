using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private float speed = 6f; // Скорость движения
    private int lives = 5; // Жизни героя
    public float health;
    public int numOfHearts;
    public Image[] hearts;
    private float jumpForce = 15f; // Сила прыжка

    public Text goldText;

    public int gold = 0; // Goldanzahl des Helden

    public GameObject loseScene;
    public GameObject wonScene;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded = false;
    private Animator anim;

    public static Hero Instance {get; set;}
    private States State
    {
        get{return (States)anim.GetInteger("state");}
        set{anim.SetInteger("state",(int)value);}
    }

    private void Awake()
    {
        Debug.Log("abc2");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of Hero detected.");
        }
        // Получаем компоненты при инициализации
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
            if (horizontalInput != 0) // Если есть движение по оси X
            {
                State = States.run; // Устанавливаем состояние бега
            }
            else
            {
                State = States.idle; // Если нет движения, устанавливаем состояние ожидания
            }
        }
        
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (transform.position.y < -300)
        {
            GameOver();
        }

        
    }

    private void FixedUpdate()
    {
        if(health > numOfHearts)
        {
           lives=numOfHearts;
        }
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
        if (horizontalInput != 0)
        {
            sprite.flipX = horizontalInput < 0;
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        State = States.jump;
    }


    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.down * 0.5f, 0.3f);
        isGrounded = colliders.Length > 1; 

        if(!isGrounded) State = States.jump;
    }

    public void GetDamage()
    {
        lives -=1;
        Debug.Log(lives);
        UpdateHearts();

        if(lives<=0)
        {
            GameOver();
        }
    }
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold collected: " + gold);
        UpdateGoldUI();
    }
    public void GameOver()
    {
        // Hier kannst du die GameOver-Szene laden
        loseScene.SetActive(true);

    }
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].enabled = true; // Herz anzeigen, wenn Leben vorhanden
            }
            else
            {
                hearts[i].enabled = false; // Herz ausblenden, wenn kein Leben vorhanden
            }
        }
    }
    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = gold.ToString();
        }
        else
        {
            Debug.LogWarning("GoldText UI element not assigned!");
        }
    }
}

public enum States
{
    idle,
    run,
    jump
}
