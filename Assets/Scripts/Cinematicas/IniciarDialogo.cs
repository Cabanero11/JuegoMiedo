using UnityEngine;
using UnityEngine.Playables; // Importante para trabajar con Timeline

public class IniciarCinematica : MonoBehaviour
{
    private ControladorDeDialogo controladorDeDialogo;

    public Dialogo dialogo;


    private void Awake()
    {
        controladorDeDialogo = GameObject.FindGameObjectWithTag("DialogoManager").GetComponent<ControladorDeDialogo>();

        if(controladorDeDialogo == null)
        {
            Debug.LogWarning("NO controladorDeDialogo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controladorDeDialogo.IniciarDialogo(dialogo);
        }
    }
}