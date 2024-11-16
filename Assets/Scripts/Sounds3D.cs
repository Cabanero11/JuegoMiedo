using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class Sounds3D : MonoBehaviour
{
    [Header("Audio a playear en 3D")]
    public AudioClip ambientSound;
    [Range(0f, 1f)]
    public float volumen;
    private AudioSource audioSource;
    private BoxCollider boxCollider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        audioSource.clip = ambientSound;
        audioSource.volume = volumen;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // Para sonido 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 15f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
