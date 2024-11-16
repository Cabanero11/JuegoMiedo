using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private HashSet<string> llavesObtenidas = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void RecogerLlave(string keyID)
    {
        if (!llavesObtenidas.Contains(keyID))
        {
            llavesObtenidas.Add(keyID);
            Debug.Log("Llave " + keyID + " recogida.");
        }
    }

    public bool TieneLlave(string keyID)
    {
        return llavesObtenidas.Contains(keyID);
    }
}
