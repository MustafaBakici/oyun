using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);  
    }



    // Start is called before the first frame update
    void Start()
    {
        foreach (var sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = false;

        }
        AudioManager.instance.Play("mainmusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds,sound => sound.name == name);
        if (s == null) 
        { Debug.Log("Ses: " + name + "bulunamadý"); }
        s.audioSource.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        { Debug.Log("Ses: " + name + "bulunamadý"); }
        s.audioSource.Stop();
    }
}
