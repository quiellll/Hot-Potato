using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Barras de Volumen")]
    public Button[] musicBars; // Botones de las barras de música
    public Button[] soundBars; // Botones de las barras de sonido
    public Button restoreButton; // Botón para restaurar

    private const int MAX_BARS = 5;
    private int defaultVolume = 3; // Valor por defecto (3 barras activas)

    void Start()
    {
        // Inicializa los valores por defecto
        // Initialize bars based on current AudioManager volumes
        InitializeVolumeDisplay();

        // Add listeners to the bars and restore button
        foreach (var bar in musicBars)
            bar.onClick.AddListener(() => UpdateBars(musicBars, bar, true));

        foreach (var bar in soundBars)
            bar.onClick.AddListener(() => UpdateBars(soundBars, bar, false));

        restoreButton.onClick.AddListener(RestoreDefaults);
    }
    private void InitializeVolumeDisplay()
    {
        if (AudioManager.Instance != null)
        {
            // Convert the 0-1 volume range to bar count (0-5)
            int musicBarCount = Mathf.RoundToInt(AudioManager.Instance.GetMusicVolume() * MAX_BARS);
            int sfxBarCount = Mathf.RoundToInt(AudioManager.Instance.GetSFXVolume() * MAX_BARS);

            SetVolume(musicBars, musicBarCount);
            SetVolume(soundBars, sfxBarCount);
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found!");
            SetVolume(musicBars, defaultVolume);
            SetVolume(soundBars, defaultVolume);
        }
    }

    // Actualiza las barras de volumen según el botón seleccionado
    void UpdateBars(Button[] bars, Button selectedBar, bool isMusic)
    {
        int index = System.Array.IndexOf(bars, selectedBar);
        int activeCount = index + 1;

        // Update visual display
        SetVolume(bars, activeCount);

        // Update actual volume in AudioManager
        if (AudioManager.Instance != null)
        {
            // Convert bar count to volume (0-1 range)
            float volume = (float)activeCount / MAX_BARS;

            if (isMusic)
                AudioManager.Instance.SetMusicVolume(volume);
            else
                AudioManager.Instance.SetSFXVolume(volume);
        }
    }

    // Activa/desactiva las barras según el nivel de volumen
    void SetVolume(Button[] bars, int activeCount)
    {
        for (int i = 0; i < bars.Length; i++)
        {
            var image = bars[i].GetComponent<Image>();
            image.color = (i < activeCount) ? Color.red : Color.white; // Cambia el color
        }
    }

    // Restaura los valores por defecto
    void RestoreDefaults()
    {
        SetVolume(musicBars, defaultVolume);
        SetVolume(soundBars, defaultVolume);

        if (AudioManager.Instance != null)
        {
            float defaultVolumeNormalized = (float)defaultVolume / MAX_BARS;
            AudioManager.Instance.SetMusicVolume(defaultVolumeNormalized);
            AudioManager.Instance.SetSFXVolume(defaultVolumeNormalized);
        }
    }
    private void OnEnable()
    {
        InitializeVolumeDisplay();
    }
}
