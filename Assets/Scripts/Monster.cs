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

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start() => dir = transform.right;

    private void Update() => Move();

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + 0.7f * dir.x * transform.right, 0.1f);

        if (colliders.Length > 0)
        {
            dir *= -1f;
            sprite.flipX = dir.x < 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        if (dir.x != 0)State = (dir.x != 0) ? MonsterStates.run : MonsterStates.idle;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Hero.Instance != null && collision.gameObject == Hero.Instance.gameObject) Hero.Instance.GetDamage();
    }
}

public enum MonsterStates
{
    idle,
    run
}

