using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuYOpciones : MonoBehaviour
{
    // Referencias a los objetos de la UI
    [Header("Paneles")]
    public GameObject mainMenuPanel;    // Panel del menú principal
    public GameObject optionsPanel;     // Panel de opciones

    [Header("Nivel a cargar")]
    public string nivel1;

    [Header("Opciones")]
    public Slider volumeSlider;             // Slider para el volumen
    public TextMeshProUGUI volumenTexto;
    public TMP_Dropdown resolutionDropdown; // Dropdown para las resoluciones

    private Resolution[] resolutions;

    private void Start()
    {
        // Inicializar el menú principal
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        // Configurar las opciones del volumen
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); // Valor por defecto: 1 (máximo)
            volumeSlider.value = savedVolume;
            AudioListener.volume = savedVolume;

            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
        }

        // Configurar las opciones de resolución
        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            int currentResolutionIndex = 0;
            var options = new System.Collections.Generic.List<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                string resolutionOption = $"{resolutions[i].width} x {resolutions[i].height}";
                options.Add(resolutionOption);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", currentResolutionIndex);
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }
    }

    // Función para Jugar (cargar escena principal)
    public void PlayGame()
    {
        Debug.Log("Suerte..."); 
        SceneManager.LoadScene(nivel1); 
    }

    // Función para salir del juego
    public void QuitGame()
    {
        Debug.Log("Bye bye...");
        Application.Quit();
    }



    // Función para establecer el volumen
    public void SetVolume(float volume)
    {
        // Convertir el volumen de 0.0-1.0 (Slider) a 0-10
        float mappedVolume = volume * 10f; // Escala el rango a 0-10

        // Ajustar el volumen del AudioListener (permanece en el rango 0.0-1.0)
        AudioListener.volume = volume;

        // Actualizar el texto del volumen como un número entero
        volumenTexto.text = Mathf.RoundToInt(mappedVolume).ToString();

        // Guardar el volumen en PlayerPrefs (en el rango 0-10)
        PlayerPrefs.SetInt("Volume", Mathf.RoundToInt(mappedVolume));
    }

    // Función para establecer la resolución
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    // Función para alternar pantalla completa
    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
