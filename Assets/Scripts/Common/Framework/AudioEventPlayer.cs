using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventPlayer : MonoBehaviour {

    public List<AudioEvent> audioEvents;
    public AudioSource audioSource;

    public void Play()
    {
        foreach (AudioEvent ae in audioEvents)
        {
            ae.Play(audioSource);
        }
    }
}
