using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<AudioClip> audioList = new List<AudioClip>();
    // Start is called before the first frame update

    public void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip, 1.0f);
    }
}
