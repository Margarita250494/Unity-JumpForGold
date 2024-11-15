using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance {get; private set;}
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource coinAudio;
    [SerializeField] private AudioSource lifeAudio;
    [SerializeField] private AudioSource loseAudio;
    [SerializeField] private AudioSource winAudio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    public void PlayJumpAudio() => jumpAudio.Play();
    public void PlayCoinAudio() => coinAudio.Play();
    public void PlayLifeAudio() => lifeAudio.Play();
    public void PlayLoseAudio() => loseAudio.Play();
    public void PlayWinAudio() => winAudio.Play();
}
