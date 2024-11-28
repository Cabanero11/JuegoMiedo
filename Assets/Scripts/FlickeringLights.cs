using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FlickeringLights : MonoBehaviour
{
    [Header("Referencia y variables del flickering")]
    public Light lightSource;              // La luz que quieres parpadear
    public AudioSource audioSource;        // Componente de AudioSource
    public float cooldownMinimo = 1.5f;    // Tiempo m�nimo entre parpadeos
    public float cooldownMaximo = 4f;      // Tiempo m�ximo entre parpadeos

    private bool isInsideTrigger = false;  // Indica si hay algo dentro del trigger

    private void Start()
    {
        // Comienza el parpadeo
        StartCoroutine(Flickering());
    }

    private IEnumerator Flickering()
    {
        while (true)
        {
            if (isInsideTrigger)
            {
                // Espera un tiempo aleatorio entre cooldownMinimo y cooldownMaximo
                float randomCooldown = Random.Range(cooldownMinimo, cooldownMaximo);
                yield return new WaitForSeconds(randomCooldown);

                // Alterna la luz
                lightSource.enabled = !lightSource.enabled;

                // Si la luz se apaga, reproduce el sonido
                if (!lightSource.enabled && audioSource != null)
                {
                    audioSource.PlayOneShot(audioSource.clip);
                }
            }
            else
            {
                // Si nadie est� en el trigger, espera un peque�o momento antes de verificar de nuevo
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Activa el parpadeo si algo entra en el �rea
        isInsideTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Detiene el parpadeo si todo sale del �rea
        isInsideTrigger = false;
        lightSource.enabled = true; // Asegura que la luz quede encendida cuando no hay nadie
    }
}
