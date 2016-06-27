using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public AudioClip hit, miss;
    public List<AudioClip> menuSoundtrack;
    public List<AudioClip> gameSoundtrack;

    public AudioSource backgroundSoundtrack;
    public AudioSource soundEffectSource;

    private System.Random rnd;

    public enum Type
    {
        Hit,
        Miss,
    }

	// Use this for initialization
	void Awake () {
        rnd = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayMenuSoundtrack()
    {
        backgroundSoundtrack.clip = menuSoundtrack[rnd.Next(0, menuSoundtrack.Count)];
        backgroundSoundtrack.Play();
    }

    public void PlayGameSoundtrack()
    {
        backgroundSoundtrack.clip = gameSoundtrack[rnd.Next(0, gameSoundtrack.Count)];
        backgroundSoundtrack.Play();
    }

    public void PlaySFX(Type type)
    {
        switch (type)
        {
            case Type.Hit:
                soundEffectSource.clip = hit;
                soundEffectSource.Play();
                break;
            case Type.Miss:
                soundEffectSource.clip = miss;
                soundEffectSource.Play();
                break;
        }
        
    }
}
