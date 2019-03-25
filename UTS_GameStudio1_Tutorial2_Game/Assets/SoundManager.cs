using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             

    private bool soundPlaying = false;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        if (!soundPlaying)
        {
            soundPlaying = true;
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            efxSource.clip = clip;

            //Play the clip.
            efxSource.Play();
            soundPlaying = false;
        }
        
    }
    //Used to play single sound clips.
    public void PlayLoop(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            musicSource.clip = clip;

        //Play the clip.
        musicSource.Play();
        }

    public void StopSound()
    {
        musicSource.Stop();
    }

}


