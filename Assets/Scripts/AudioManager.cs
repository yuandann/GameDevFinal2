using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public AudioSource source;

    public Dictionary<string, AudioClip> sounds;
    [Serializable]
    public struct ClipwName
    {
        private string name;
        private AudioClip clip;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
