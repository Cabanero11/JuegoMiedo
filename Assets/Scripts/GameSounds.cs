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
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float spatialBlend = 1f; 
        public bool loop = false;
    }

    [Header("Lista de sonidos")]
    public List<Sound> sounds = new List<Sound>();

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
    }

    /**
     * Funcion para playear un sonido con opciones 3D creando un nuevo AudioSource cada vez.
     * La posicion del sonido se establece en la posicion del GameObject que llama a la funcion.
     */
    public static void Play(string soundName, float pitchMin = 0.9f, float pitchMax = 1.1f, float minDistance = 1f, float maxDistance = 20f, Vector3? position = null)
    {
        if (instance == null)
        {
            Debug.LogError("GameSounds instance not found!");
            return;
        }

        Sound sound = instance.sounds.Find(s => s.name == soundName);

        if (sound != null)
        {
            // Crear un nuevo GameObject para el AudioSource
            GameObject soundObject = new GameObject("Sound_" + soundName);
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            // Configuracion de la posicion del sonido
            soundObject.transform.position = position ?? instance.transform.position;

            // Variables del sonido
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.spatialBlend = sound.spatialBlend;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.loop = sound.loop;


            audioSource.Play();

            // Destruir el GameObject cuando el audio termine
            if (!sound.loop)
            {
                Destroy(soundObject, sound.clip.length / audioSource.pitch);
            }
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
}
