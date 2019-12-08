using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public AudioSource source;

    public List<ClipwName> clips;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        for (int i = 0; i < clips.Count; i++)
        {
            sounds.Add(clips[i].name, clips[i].clip);
        }
    }
    
    public Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    
    [Serializable]
    public struct ClipwName
    {
        public string name;
        public AudioClip clip;
    }
    public void PlayClip(string clipName)
    {
        source.PlayOneShot(sounds[clipName]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
