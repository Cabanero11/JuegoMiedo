using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Singleton")]
    public static PlayerCamera instance;

    [Header("Parameters")]
    public float sensibilityX = 550f;
    public float sensibilityY = 350f;
    public bool cameraRespawn = false;

    public Transform orientation;
    public Transform camHolder;

    public float transitionTime = 0.25f;


    float xRotation, yRotation;

    [Header("PointOfViewCamera")]
    public Camera povCam;  


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        { 
            Destroy(instance);
            Debug.LogWarning("Se ha borrado la c�mara de jugador antigua");
            instance = this;
        }
    }

    void Start()
    {
        // en el medio y invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibilityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensibilityY;

        yRotation += mouseX;
        xRotation -= mouseY;

        // no mas de 90�
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        if (orientation != null)
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        else if (Player.instance != null)
            orientation = Player.instance.GetComponent<PlayerMovement>().orientation;
    }

    
}
