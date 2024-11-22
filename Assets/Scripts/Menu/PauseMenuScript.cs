using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private bool isGamePaused = false;
    public static bool isGamePausedStatic;

    [Header("Referencias")]
    public GameObject pauseMenuUI;
    public GameObject reticula;

    [Header("Opciones de Audio")]
    public Slider volumeSlider; // Slider para el volumen
    public TextMeshProUGUI volumenTexto;


    private void Awake()
    {
        pauseMenuUI.SetActive(false); // No pausa, al inicio xd
    }

    private void Start()
    {
        // Configurar el slider de volumen
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // Valor por defecto: 1 (m�ximo)
            volumeSlider.value = savedVolume;
            AudioListener.volume = savedVolume;

            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
        }
    }


    public void VolverAlMenu()
    {
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;

        //GameManager.Instance.gameState = GameState.menu;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Sali del juego :(");
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }



    public void ContinueGame()
    {
        reticula.SetActive(true);
        //pauseMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        //GameManager.Instance.gameState = GameState.level;
    }

    public void PauseGame()
    {
        reticula.SetActive(false);
        pauseMenuUI.SetActive(true);

        //pauseMenuUI.SetActive(true);

        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

    }



    public void TogglePauseMenu()
    {
        if (isGamePaused)
        {
            ContinueGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Funci�n para establecer el volumen
    public void SetVolume(float volume)
    {
        // Ajustar el volumen del AudioListener
        AudioListener.volume = volume;

        // Actualizar el texto del volumen
        if (volumenTexto != null)
        {
            float mappedVolume = volume * 10f; // Escala el rango a 0-10
            volumenTexto.text = Mathf.RoundToInt(mappedVolume).ToString();
        }

        // Guardar el volumen en PlayerPrefs
        PlayerPrefs.SetFloat("Volume", volume);
    }
}

