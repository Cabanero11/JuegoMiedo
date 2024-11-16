using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerLlave : MonoBehaviour
{
    [Header("Nombre de la llave")]
    public string keyID;
    [Range(0f, 1f)]
    public float volume;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                GameSounds.Play("Key", volume);
                gameManager.RecogerLlave(keyID);
                Debug.Log("llave: " + keyID + " recogida.");
                Destroy(gameObject); // Destruye la llave al recogerla
            }
            else
            {
                Debug.LogError("No se encontró el GameManager.");
            }
        }
    }
}
