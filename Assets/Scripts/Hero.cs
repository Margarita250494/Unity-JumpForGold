using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 4f; // Скорость движения
    [SerializeField] private int lives = 5; // Жизни героя
    [SerializeField] private float jumpForce = 9f; // Сила прыжка

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded = false;

    private void Awake()
    {
        // Получаем компоненты при инициализации
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Проверка направления
        float horizontalInput = Input.GetAxis("Horizontal");
        UpdateSpriteDirection(horizontalInput);
        
        // Проверка на прыжок
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        CheckGround(); // Проверяем, на земле ли персонаж
        Move();
    }

    private void Move()
    {
        // Задаем скорость для движения по оси X
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void UpdateSpriteDirection(float horizontalInput)
    {
        // Изменяем направление спрайта в зависимости от движения
        if (horizontalInput != 0)
        {
            sprite.flipX = horizontalInput < 0;
        }
    }

    private void Jump()
    {
        // Применяем силу к Rigidbody2D для прыжка
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    private void CheckGround()
    {
        // Проверка, касается ли герой земли
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.down * 0.5f, 0.3f);
        isGrounded = colliders.Length > 1; // Предполагается, что есть более одного коллайдера
    }
}
