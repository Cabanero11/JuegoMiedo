using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Variables interaccion")]
    public float interactionRange = 3f;     
    public LayerMask interactionLayer;      
    public TextMeshProUGUI textoF;
    public Camera playerCamera;

    void Start()
    {
        textoF.gameObject.SetActive(false); 

        if(playerCamera == null)
        {
            Debug.LogError("NO Camara en PlayerInteraction.cs");
        }
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactionLayer))
        {
            // Verifica si el objeto tiene el tag "Interactuar".
            if (hit.collider.CompareTag("Interactuar"))
            {
                textoF.text = "F para interactuar";
                textoF.gameObject.SetActive(true);

                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Interact(hit.collider.gameObject);
                }
            }
        }
        else
        {
            // Oculta el texto si no estás mirando un objeto con el tag.
            textoF.gameObject.SetActive(false);
        }
    }

    void Interact(GameObject obj)
    {
        Debug.Log("Interactue con " + obj.name);

        /**
        AudioSource audioSource = obj.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Interact");
        }
        */
    }
}
