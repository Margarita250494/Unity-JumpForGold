using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public int requiredGold = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prüft, ob der kollidierte GameObject der Hero ist
        Hero hero = collision.gameObject.GetComponent<Hero>();

        if (hero != null) // Überprüft, ob ein Hero-Objekt vorhanden ist
        {
            // Überprüfen, ob der Hero genug Gold gesammelt hat
            if (hero.gold >= requiredGold)
            {
                Debug.Log("Gewonnen! Der Hero hat genug Gold und hat den Baum erreicht.");
                Hero.Instance.wonScene.SetActive(true);
                
            }
            else
            {
                Debug.Log("Nicht genug Gold! Sammle mehr Gold, um zu gewinnen.");
            }
        }
    }
}
