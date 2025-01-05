using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameActive { get; private set; } = true;
    public string CurrentPlayer;


    public TouchManager touchManager;
    public BombMechanic bombMechanic;

    [Header("UI Elements")]
    public TMPro.TextMeshProUGUI PlayerOneText;
    public TMPro.TextMeshProUGUI PlayerTwoText;

    private List<Player> playerList = new List<Player>();
    private int currentPlayerIndex = 0;

    public Player topPlayer;    // Player on the top side
    public Player bottomPlayer; // Player on the bottom side
    public bool isWaitingForRelease = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            touchManager = FindFirstObjectByType<TouchManager>();
            bombMechanic = FindFirstObjectByType<BombMechanic>();

            Debug.Log("GameManager initialized. IsGameActive: " + IsGameActive);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePlayers();
        StartNewRound();
    }

    private void InitializePlayers()
    {
        playerList.Add(new Player("Juan"));
        playerList.Add(new Player("Paco"));
        playerList.Add(new Player("Carlota"));
        playerList.Add(new Player("Pilar"));
        ShufflePlayers();
    }

    private void ShufflePlayers()
    {
        for (int i = playerList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = playerList[i];
            playerList[i] = playerList[j];
            playerList[j] = temp;
        }
    }

    public void UpdateUI()
    {
        PlayerOneText.text = topPlayer?.Name ?? "Waiting...";
        PlayerTwoText.text = bottomPlayer?.Name ?? "Tap to join!";
    }

    public void StartNewRound()
    {
        IsGameActive = false;
        currentPlayerIndex = 0;
        topPlayer = playerList[currentPlayerIndex];
        bottomPlayer = null;
        isWaitingForRelease = false;
        UpdateUI();
        Debug.Log($"New round started with {topPlayer.Name}");
    }

    public void StartGame()
    {
        if (!IsGameActive && topPlayer != null)
        {
            IsGameActive = true;
            bombMechanic.SetBombLive();
            Debug.Log("Game started!");
        }
    }

    public void EndGame()
    {
        IsGameActive = false;
        ShufflePlayers();
        StartNewRound();
    }

    public void SelectNextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % playerList.Count;
        return playerList[currentPlayerIndex];
    }

    public void BombExploded()
    {
        // aqu� todo lo que quieras que pase cuando explote.
        Debug.Log($"BOOM! Players lose: {CurrentPlayer}");
        EndGame();
    }
}
