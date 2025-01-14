using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public Image background;
    public Image playerImage;
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI timerText;
    public Button gameButton;
    public TextMeshProUGUI eliminationText;

    [Header("Game Settings")]
    public float roundDuration = 15f;
    public int minPlayers = 3;
    public int maxPlayers = 5;

    [Header("Game Components")]
    public BombMechanic bombMechanic;
    public TouchManager touchManager;

    [Header("Configuration")]
    [SerializeField] private GameConfiguration config;

    private Vector2 imageTextureResolution;

    // Game state tracking
    private GameState currentState;
    private float remainingTime;
    private PlayerPosition currentPlayerPosition;

    // Player management
    private List<Player> players = new List<Player>();
    private int currentPlayerIndex = 0;
    private string eliminatedPlayer = "";

    private void Awake()
    {
        SetupSingleton();
        InitializeGame();
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        // Find required components
        bombMechanic = FindFirstObjectByType<BombMechanic>();
        touchManager = FindFirstObjectByType<TouchManager>();

        // Load players from configuration instead of debug players
        LoadPlayersFromConfig();
        StartNewRound();

        eliminationText.gameObject.SetActive(false);
    }

    private void SetupGameButton()
    {
        gameButton.onClick.RemoveAllListeners();
        gameButton.onClick.AddListener(StartPlaying);
        gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
    }

    //private void CreateDebugPlayers()
    //{
    //    players.Clear();
    //    for (int i = 1; i <= 4; i++)
    //    {
    //        players.Add(new Player($"Player {i}", null, null));
    //    }
    //    ShufflePlayers();
    //}

    private void LoadPlayersFromConfig()
    {
        players.Clear();

        // Check if we have enough players
        if (config.players.Count < minPlayers)
        {
            Debug.LogWarning($"Not enough players in configuration! Minimum required: {minPlayers}");
            return;
        }

        // Load only up to maxPlayers
        int playerCount = Mathf.Min(config.players.Count, maxPlayers);

        for (int i = 0; i < playerCount; i++)
        {
            var playerData = config.players[i];
            var player = gameObject.AddComponent<Player>();
            player.Initialize(playerData.Name, playerData.Background, playerData.PlayerFace);
            players.Add(player);
            SetupImageResolution(playerData.GetImageResolution());
        }

        ShufflePlayers();
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    private void ShufflePlayers()
    {
        for (int i = players.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = players[i];
            players[i] = players[j];
            players[j] = temp;
        }
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;
        if (timerText != null)
        {
            timerText.text = $"{Mathf.Ceil(remainingTime):0}";
        }

        if (remainingTime <= 0)
        {
            // AQUI ES DONDE SE ACABA EL TIEMPO. PARAR EL JUEGO Y PASAR A SIGUIENTE RONDA. HACER COSAS DE UI AQUI
            StopTimer();
            HandleExplosion();
        }
    }

    private void StopTimer()
    {
        if (timerText != null)
            timerText.text = "--:--";
    }

    // Player Management
    private Player GetCurrentPlayer() => players[(currentPlayerIndex) % players.Count];
    private Player GetNextPlayer() => players[(currentPlayerIndex + 1) % players.Count];

    private void AdvanceToNextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        UpdateUI();
    }

    // Game Flow Methods
    public void StartNewRound()
    {
        if (players.Count == 1) EndGame();

        SetupGameButton();
        SetupFirstPlayer();
        currentState = GameState.WaitingToStart;
        remainingTime = roundDuration;
    }

    public void SetupFirstPlayer()
    {
        var player = players[0];

        currentPlayerText.text = player.Name;
        playerImage.sprite = player.PlayerFace;
        background.sprite = player.Background;
    }

    private void StartPlaying()
    {
        if (currentState == GameState.WaitingToStart)
        {
            // quizas aqui poner que el texto del boton cambie y tambien le cambio la funcion.
            UpdateUI();
            currentState = GameState.Playing;
            bombMechanic.SetBombLive();
            AdvanceToNextPlayer();
            eliminationText.gameObject.SetActive(false);
        }
    }

    public GameState GetCurrentState() => currentState;

    public void HandleExplosion()
    {
        // aqui hacer las cosas de explosion, como poner un ruidito o indicar q ha perdido.
        EliminateCurrentPlayer();
        currentState = GameState.GameOver;
        StartNewRound();
    }

    private void EliminateCurrentPlayer()
    {
        EliminatePlayer(GetCurrentPlayer());
        ShowEliminationText();
    }

    private void EliminatePlayer(Player player)
    {
        player.Eliminate();
        players.Remove(player);

        eliminatedPlayer = player.Name;

        if (players.Count < 2)
        {
            // aqui se puede pasar a la pantalla de victoria con el ganador
        }
    }

    private void UpdateUI()
    {
        if (currentState == GameState.GameOver)
        {
            eliminationText.gameObject.SetActive(false);
        }

        if (currentState == GameState.WaitingToStart)
        {

            gameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
            gameButton.onClick.RemoveAllListeners();
            gameButton.onClick.AddListener(AdvanceToNextPlayer);

            // guarrada pero es que sin esto en la primera ronda siempre se saltaba al jugador en 2 turno lol
            currentPlayerIndex--;
            return;
        }

        var next = GetNextPlayer();
        currentPlayerText.text = next.Name;
        background.sprite = next.Background;
        LoadPlayerPic(next);
    }

    private void LoadPlayerPic(Player p)
    {
        playerImage.sprite = p.PlayerFace;
    }

    public void SetupImageResolution(Vector2 res)
    {
        imageTextureResolution = res;

        // pillar la resolucion de la foto
        RectTransform rectTransform = playerImage.GetComponent<RectTransform>();

        // cambio la resolucion de la foto a la de la camara pero a la mitad
        rectTransform.sizeDelta = res;
    }

    private void ShowEliminationText()
    {
        eliminationText.gameObject.SetActive(true);
        eliminationText.text = ($"{eliminatedPlayer} eliminated!");
    }

    public Player GetWinner()
    {
        return players.Count > 0 ? players[0] : null;
    }

    private void EndGame()
    {
        if (players.Count > 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Winner");
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.name == "Winner")
                {
                    var winScreen = FindAnyObjectByType<WinScreen>();
                    winScreen?.UpdateWinScreen();
                }
            };
        }
    }
}