using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpConColision : MonoBehaviour
{
    public Transform ubicacionATeletransportarse;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = ubicacionATeletransportarse.position;

            Debug.Log("Jugador TPiado");

            // Sonido o cinematica
            
        }
    }
}
