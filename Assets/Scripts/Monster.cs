using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private Animator anim;

    private MonsterStates State
{
    get { return (MonsterStates)anim.GetInteger("state"); }
    set { anim.SetInteger("state", (int)value); }
}

    private void Start()
    {
        dir = transform.right;
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + 0.7f * dir.x * transform.right, 0.1f);

        if(colliders.Length > 0)
        {
            dir *= -1f;
            sprite.flipX = dir.x < 0;
        } 

        transform.position = Vector3.MoveTowards(transform.position,transform.position + dir, speed * Time.deltaTime);

        if (dir.x != 0)
        {
            State = MonsterStates.run; // Wenn das Monster sich bewegt, Zustand auf "run" setzen
        }
        else
        {
            State = MonsterStates.idle; // Wenn das Monster sich nicht bewegt, Zustand auf "idle" setzen
        }
        
    }
     private void OnCollisionEnter2D(Collision2D collision)
{

    if (collision.gameObject == Hero.Instance.gameObject)
    {
        
        Hero.Instance.GetDamage();
    }
}
}

public enum MonsterStates
{
    idle,
    run
}

