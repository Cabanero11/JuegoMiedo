using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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
    [Range(0f, 0.5f)]
    public float tiempoEsperaTrasAcabarDialogo = 1f;

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

        // Recorremos todas las líneas del diálogo
        foreach (char letra in dialogoActual.lineasDeDialogo[indiceLinea])
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadDeCaracteres);
        }

        // Esperamos 1s antes del siguiente dialogo
        yield return new WaitForSeconds(tiempoEsperaTrasAcabarDialogo);

        // Pasar a la siguiente linea
        indiceLinea++;

        // Hasta que quede texto 
        if (indiceLinea < dialogoActual.lineasDeDialogo.Length)
        {
            StartCoroutine(MostrarDialogo());
        }
        else // Acabio el dialogo
        {
            dialogoActivo = false;
            textBox.SetActive(false);
            Debug.Log("Fin del diálogo");
        }
    }


}
