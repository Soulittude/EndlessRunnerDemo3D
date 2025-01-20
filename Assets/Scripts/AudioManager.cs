using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isLoop;
        }

        PlaySound("MainTheme");
    }

    public void PlaySound(string soundName)
    {
        foreach(Sound s in sounds)
        {
            if(s.name == soundName)
                s.source.Play();
        }
    }
    
    public void StopSound(string soundName)
    {
        foreach(Sound s in sounds)
        {
            if(s.name == soundName)
                s.source.Stop();
        }
    }
}
