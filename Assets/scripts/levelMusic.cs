using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class levelmusic : MonoBehaviour
{
    

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambianceSource;

    public List<AudioClip> bgmusic = new List<AudioClip>();
    public AudioClip nightAmbience;

    private void Start()
    {
        PlayBackgroundMusic();
        PlayNightAmbience();
    }

    private void PlayBackgroundMusic()
    {
       
        
            musicSource.clip = bgmusic[Random.Range(0,2)];
            musicSource.loop = true;
            musicSource.Play();
        
    }

    private void PlayNightAmbience()
    {
        if (nightAmbience != null)
        {
            ambianceSource.clip = nightAmbience;
            ambianceSource.loop = true;
            ambianceSource.Play();
        }
    }
}
