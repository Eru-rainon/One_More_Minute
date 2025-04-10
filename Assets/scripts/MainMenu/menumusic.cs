using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.loop = true;
            bgMusic.Play();
        }
    }

    public void SetVolume(float volume)
    {
        bgMusic.volume = volume;
    }

    public void StopMusic()
    {
        bgMusic.Stop();
    }
}
