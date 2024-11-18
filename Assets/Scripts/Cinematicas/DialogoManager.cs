using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class ControladorDeDialogo : MonoBehaviour
{
    /**
     * HOW TO DIALOGOS
     * 1. Crear un Dialogo con -> Boton Derecho -> SistemaDeDialogo -> Dialogo y añadirle dialogos
     * 2. Mediante un collider (isTrigger) y IniciarDialogo, pasar el dialogo a reproducir
     * 3. Profit 
     */


    public GameObject textBox; 
    public TextMeshProUGUI textoDialogo;
    [Range(0f, 0.5f)]
    public float velocidadDeCaracteres = 0.05f;
    [Range(0f, 1f)]
    public float tiempoEsperaTrasAcabarDialogo = 1f;
    public float velocidadEfectoOla = 3f;
    public float amplitudEfectoOnda = 0.5f;


    public bool dialogoActivo = false;

    private int indiceLinea = 0;
    private Dialogo dialogoActual;

    void Start()
    {
        textoDialogo.text = "";
        textBox.SetActive(false);
    }


    public void IniciarDialogo(Dialogo nuevoDialogo)
    {
        dialogoActual = nuevoDialogo;
        indiceLinea = 0;
        dialogoActivo = true;
        textBox.SetActive(true);
        StartCoroutine(MostrarDialogo());
    }

    private IEnumerator MostrarDialogo()
    {
        // Borramos cualquier texto anterior
        textoDialogo.text = "";

        // Obtenemos la línea del diálogo con las etiquetas HTML
        string linea = dialogoActual.lineasDeDialogo[indiceLinea];
        int longitudLinea = linea.Length;
        int i = 0;

        // Variables para el efecto de onda
        bool efectoOnda = false;
        List<int> indicesOnda = new List<int>();

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
                    i = cierreEtiqueta + 1; // Avanza el índice al final de la etiqueta
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
                // Si es un carácter normal, se agrega de uno en uno
                textoDialogo.text += linea[i];

                // Si el efecto está activo, agregar el índice de la letra
                if (efectoOnda)
                {
                    indicesOnda.Add(textoDialogo.text.Length - 1);
                }

                i++;
                yield return new WaitForSeconds(velocidadDeCaracteres);
            }
        }


        // Esperamos antes de continuar con la siguiente línea
        yield return new WaitForSeconds(tiempoEsperaTrasAcabarDialogo);

        // Detener la animación de ola al terminar la línea
        dialogoActivo = false;

        // Pasar a la siguiente línea
        indiceLinea++;

        if (indiceLinea < dialogoActual.lineasDeDialogo.Length)
        {
            StartCoroutine(MostrarDialogo());
        }
        else
        {
            textBox.SetActive(false);
            Debug.Log("Fin del diálogo");
        }
    }

    




}
