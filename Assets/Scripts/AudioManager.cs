using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }


    public float minpitch = 0.8f;
    public float maxpitch = 1.2f;

    public AudioSource efxsource;
    public AudioSource bgSource;

    void Awake()
    {
        _instance = this;
    }

    public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(minpitch, maxpitch);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        efxsource.clip = clip;
        efxsource.pitch = pitch;
        efxsource.Play();
    }

    public void StopBGM()
    {
        bgSource.Stop();
    }
}
    


