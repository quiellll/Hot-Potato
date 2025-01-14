using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip buttonClickSound; 

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.2f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 0.7f;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            LoadVolumeSettings();
            StartBackgroundMusic();
            SetupButtonSounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        // Create music source if not assigned
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.parent = transform;
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        // Create SFX source if not assigned
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.parent = transform;
            sfxSource = sfxObj.AddComponent<AudioSource>();
        }
    }

    private void SetupButtonSounds()
    {
        // Find all buttons in the scene and add click sound
        AddClickSoundToButtons();

        // Subscribe to scene load event to setup buttons in new scenes
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        AddClickSoundToButtons();
    }

    private void AddClickSoundToButtons()
    {
        UnityEngine.UI.Button[] buttons = FindObjectsOfType<UnityEngine.UI.Button>();
        foreach (var button in buttons)
        {
            // Remove any existing click sound listeners to avoid duplicates
            button.onClick.RemoveListener(PlayButtonClickSound);
            // Add the click sound listener
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound, sfxVolume);
        }
    }
    private void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.2f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.7f);

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    public void StartBackgroundMusic()
    {
        if (backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            sfxSource.PlayOneShot(explosionSound, sfxVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        SaveVolumeSettings();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
