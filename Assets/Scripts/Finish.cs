using UnityEngine;

public class Finish : MonoBehaviour
{
    public int requiredGold = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Hero hero = collision.gameObject.GetComponent<Hero>();

        if (hero != null) 
        {
            if (hero.gold >= requiredGold)
            {

                Debug.Log("Gewonnen! Der Hero hat genug Gold und hat den Baum erreicht.");
                Hero.Instance.wonScene.SetActive(true);
                Hero.Instance.wonAudio.Play();
                
            }
            else
            {
                Debug.Log("Nicht genug Gold! Sammle mehr Gold, um zu gewinnen.");
            }
        }
    }
}
