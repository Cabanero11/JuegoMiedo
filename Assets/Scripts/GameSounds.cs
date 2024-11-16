using UnityEngine;
using System.Collections.Generic;

public class GameSounds : MonoBehaviour
{
    public static GameSounds instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Lista de sonidos")]
    public List<Sound> sounds = new List<Sound>();
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameSounds exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    /**
     * Funcion para playear un sonido con pitch random
     */
    public static void Play(string soundName, float volume, float pitchMin = 0.9f, float pitchMax = 1.1f)
    {
        if (instance == null)
        {
            Debug.LogError("GameSounds instance not found!");
            return;
        }

        Sound sound = instance.sounds.Find(s => s.name == soundName);

        if (sound != null)
        {
            instance.audioSource.clip = sound.clip;
            instance.audioSource.volume = volume;
            instance.audioSource.pitch = Random.Range(pitchMin, pitchMax);
            instance.audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
}
