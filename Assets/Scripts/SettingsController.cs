using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Barras de Volumen")]
    public Button[] musicBars; // Botones de las barras de m�sica
    public Button[] soundBars; // Botones de las barras de sonido
    public Button restoreButton; // Bot�n para restaurar

    private int defaultVolume = 3; // Valor por defecto (3 barras activas)

    void Start()
    {
        // Inicializa los valores por defecto
        SetVolume(musicBars, defaultVolume);
        SetVolume(soundBars, defaultVolume);

        // A�ade listeners a las barras y al bot�n de restaurar
        foreach (var bar in musicBars)
            bar.onClick.AddListener(() => UpdateBars(musicBars, bar));

        foreach (var bar in soundBars)
            bar.onClick.AddListener(() => UpdateBars(soundBars, bar));

        restoreButton.onClick.AddListener(RestoreDefaults);
    }

    // Actualiza las barras de volumen seg�n el bot�n seleccionado
    void UpdateBars(Button[] bars, Button selectedBar)
    {
        int index = System.Array.IndexOf(bars, selectedBar);
        SetVolume(bars, index + 1); // Activa las barras hasta la seleccionada
    }

    // Activa/desactiva las barras seg�n el nivel de volumen
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
    }
}
