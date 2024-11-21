using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerLlave : MonoBehaviour
{
    [Header("Nombre de la llave")]
    public string keyID;
    [Header("Particula Hijo")]
    public GameObject particulaHalo;
    [Range(0f, 1f)]
    public float volume;
    public float alturaAbajoHalo = 1f;

    private void Awake()
    {
        // Poner el halo en la llave
        particulaHalo.transform.position = new Vector3(transform.position.x, transform.position.y - alturaAbajoHalo, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                GameSounds.Play("Key", volume, 1.1f, 1f, 20f, transform.position);

                gameManager.RecogerLlave(keyID);
                Debug.Log("llave: " + keyID + " recogida.");
                Destroy(gameObject); // Destruye la llave al recogerla
                Destroy(particulaHalo);
            }
            else
            {
                Debug.LogError("No se encontró el GameManager.");
            }
        }
    }
}
