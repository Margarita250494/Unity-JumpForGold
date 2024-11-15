using UnityEngine;

public class Gold : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.AddGold(1);
            Destroy(gameObject);
        }
    }


}

