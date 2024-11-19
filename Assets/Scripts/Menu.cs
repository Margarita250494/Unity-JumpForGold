using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    public void Restart() =>  SceneManager.LoadScene(1);
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}

//public void Restart() =>  SceneManager.LoadScene("Menu");