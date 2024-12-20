using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class ControladorDeDialogo : MonoBehaviour
{
    /**
     * HOW TO DIALOGOS
     * 1. Crear un Dialogo con -> Boton Derecho -> SistemaDeDialogo -> Dialogo y a�adirle dialogos
     * 2. Mediante un collider (isTrigger) y IniciarDialogo, pasar el dialogo a reproducir
     * 3. Profit 
     */

    [Header("Referencias")]
    public GameObject textBox; 
    public TextMeshProUGUI textoDialogo;
    public TextMeshProUGUI textNombreNPC;
    [Header("Variables de dialogos")]
    [Range(0f, 0.5f)]
    public float velocidadDeCaracteres = 0.05f;
    public float tiempoEsperaTrasAcabarDialogo = 1f;

    [Header("Sonidos")]
    // A�adir la lista de sonidos
    public AudioClip[] sonidosDeEscritura; 
    private AudioSource audioSource;
    [SerializeField] private float volumenSonidoEscritura = 0.5f;
    [SerializeField] private int caracteresPorSonido = 5; // Para que no suenen cada tanto xd


    public bool dialogoActivo = false;

    private int indiceLinea = 0;
    private Dialogo dialogoActual;



    void Start()
    {
        textoDialogo.text = "";
        textBox.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            Debug.LogWarning("NO AUDIO SOURCE EN DIALOGO MANAGER.");
        }

        audioSource.volume = volumenSonidoEscritura;
    }


    public void IniciarDialogo(Dialogo nuevoDialogo)
    {
        dialogoActual = nuevoDialogo;
        indiceLinea = 0;
        textBox.SetActive(true);

        // Para que solo pueda haber 1 dialogo activo a la vez
        if(dialogoActivo == false)
        {
            dialogoActivo = true;
            StartCoroutine(MostrarDialogo());
        }
    }

        

    private IEnumerator MostrarDialogo()
    {
        

        // Borramos cualquier texto anterior
        textoDialogo.text = "";
        textNombreNPC.text = "";

        // Obtenemos la l�neaDialogo del di�logo con las etiquetas HTML
        var lineaDialogo = dialogoActual.lineasDeDialogo[indiceLinea];

        // Separar el lineaDialogo en sus 2 datos
        string linea = lineaDialogo.textoLineaDialogo;
        textNombreNPC.text = lineaDialogo.nombreNPC;

        int longitudLinea = linea.Length;
        int i = 0, contadorDeCaracteres = 0;

        while (i < longitudLinea)
        {
            // Si encuentra una apertura de etiqueta '<'
            if (linea[i] == '<')
            {
                // Busca el cierre de la etiqueta '>'
                int cierreEtiqueta = linea.IndexOf('>', i);
                if (cierreEtiqueta != -1)
                {
                    // Agrega toda la etiqueta al texto de golpe
                    textoDialogo.text += linea.Substring(i, (cierreEtiqueta - i) + 1);
                    i = cierreEtiqueta + 1; // Avanza el �ndice al final de la etiqueta
                }
                else
                {
                    // Si no hay cierre, simplemente se agrega el caracter y se avanza
                    textoDialogo.text += linea[i];
                    i++;
                }
            }
            else
            {
                // Si es un caracter normal, se agrega de uno en uno
                textoDialogo.text += linea[i];

                // Reproducir el sonido cada X caracteres
                contadorDeCaracteres++;

                if (contadorDeCaracteres >= caracteresPorSonido)
                {
                    if (sonidosDeEscritura.Length > 0)
                    {
                        audioSource.PlayOneShot(sonidosDeEscritura[i % sonidosDeEscritura.Length]);
                    }

                    contadorDeCaracteres = 0; 
                }

                i++;
                yield return new WaitForSeconds(velocidadDeCaracteres);
            }
        }


        // Esperamos antes de continuar con la siguiente l�nea
        yield return new WaitForSeconds(tiempoEsperaTrasAcabarDialogo);

        // Detener la animaci�n de ola al terminar la l�nea
        dialogoActivo = false;

        // Pasar a la siguiente l�nea
        indiceLinea++;

        bool aunQuedaTexto = indiceLinea < dialogoActual.lineasDeDialogo.Length;

        if (aunQuedaTexto)
        {
            StartCoroutine(MostrarDialogo());
        }
        else
        {
            textBox.SetActive(false);
            Debug.Log("Fin del di�logo");
        }
    }

}
