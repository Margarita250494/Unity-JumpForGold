using UnityEngine;

public class Gold : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject == Hero.Instance.gameObject) 
    {
        Hero.Instance.AddGold(1);
        Destroy(gameObject);
    }

}
}
