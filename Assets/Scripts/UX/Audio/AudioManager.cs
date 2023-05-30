using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

//https://www.youtube.com/watch?v=6OT43pvUyfY&list=TLPQMjAwMzIwMjOTEVneMS2B_w&index=2&ab_channel=Brackeys
/*************************************************************************************************************
     * When you want to play audio, use the following code as an example:
     *  FindObjectOfType<AudioManager>().Play("name of sound in array");
     *  OR FindObjectOfType<AudioManager>().PlayOneShot("name of sound in array");
     * Likewise, when you want to stop audio:
     *  FindObjectOfType<AudioManager>().StopPlaying("name of sound in array");
*************************************************************************************************************/

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    //public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    void Awake()
    {
        /*
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }*/

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.PlayOneShot(s.clip);
    }

    // From Comment of Video
    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + base.name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Stop();
    }
}

//https://stackoverflow.com/questions/59459321/unity-manage-audiomanager-on-all-scenes
/*************************************************************************************************************
public class AudioManager : MonoBehaviour
{
    // if you have it in the scene you can reference these right away
    [SerializeField] private AudioData audioData;
    [SerializeField] private AudioSource audioSource;

    // backing field for actually store the Singleton instance
    private static AudioManager _instance;

    // public access for the Singleton
    // and lazy instantiation if not exists
    public static AudioManager Instance
    {
        get
        {
            // if exists directly return
            if (_instance) return instance;

            // otherwise search it in the scene
            _instance = FindObjectOfType<AudioManager>();

            // found it?
            if (_instance) return instance;

            // otherwise create and initialize it
            CreateInstance();

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeInstance(this);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void CreateInstance()
    {
        // skip if already exists
        if (_instance) return;

        InitializeInstance(new GameObject(nameof(AudioManager)).AddComponent<AudioManager>());
    }

    private static void InitializeInstance(AudioManager instance)
    {
        _instance = instance;
        DontDestroyOnLoad(gameObject);
        if (!_instance.audioSource) _instance.audioSource = _instance.AddComponent<AudioSource>();

        if (_instance.audioData) return;

        var audioDatas = Resources.FindObjectsOfType<AudioData>();
        if (audioDatas.Length == 0)
        {
            Debug.LogError("No Instance of AudioData found! Don't forget to create that ScriptableObject!");
            return;
        }

        _instance.audioData = audioDatas[0];
    }

    public void Play(SoundType soundType)
    {
        Sound s = audioData.sounds.First(sound => sound.soundType == soundType);

        s.source = audioSource;
        s.source.clip = s.clip;

        // We define volume and pitch here to be able to change them in real time
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Play();
    }
}
*************************************************************************************************************/
