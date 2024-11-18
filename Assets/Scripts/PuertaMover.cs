using UnityEngine;

public class PuertaMover : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public string keyIDRequerida;
    public float velocidadApertura = 30f; // Velocidad de incremento del ángulo
    [Header("Angulo de Y")]
    public float maxAnguloApertura = 100f; // Se puede configurar en el Inspector

    private bool jugadorDentro = false;
    private bool puertaAbierta = false;
    private Transform rotator;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        rotator = transform.Find("Rotator");


        if (rotator == null)
        {
            Debug.LogError("No se encontro el objeto 'Rotator'.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            Debug.Log("Jugador dentro de la puerta (lenny face) ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            Debug.Log("Jugador se fue de la puerta.");
        }
    }

    private void Update()
    {
        bool comprobarDentroPuerta = jugadorDentro && Input.GetKeyDown(KeyCode.F) && !puertaAbierta;

        if (comprobarDentroPuerta)
        {
            bool tieneLLave = gameManager != null && gameManager.TieneLlave(keyIDRequerida);

            if (tieneLLave)
            {
                puertaAbierta = true;
                GameSounds.Play("Puerta", 0.9f, 1.1f, 1f, 20f, transform.position);

                Debug.Log("LA PUERTA SE ABRE !...!!!");
            }
            else
            {
                Debug.Log("No tienes la llave OMG BRUH");
            }
        }

        // Rotación suave de la puerta si está abierta y no ha alcanzado el ángulo máximo
        if (puertaAbierta)
        {
            float anguloActualY = rotator.localEulerAngles.y;
            float anguloObjetivoY = (maxAnguloApertura >= 0) ? maxAnguloApertura : 360 + maxAnguloApertura;

            if (Mathf.Abs(anguloActualY - anguloObjetivoY) > 0.1f)
            {
                float nuevoAnguloY = Mathf.MoveTowardsAngle(anguloActualY, anguloObjetivoY, velocidadApertura * Time.deltaTime);
                rotator.localEulerAngles = new Vector3(rotator.localEulerAngles.x, nuevoAnguloY, rotator.localEulerAngles.z);
            }
            
        } 

    }
}
