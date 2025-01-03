using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameActive { get; private set; } = false;
    public TouchManager touchManager;
    public BombMechanic bombMechanic;

    [Header("UI Elements")]
    public TMPro.TextMeshProUGUI PlayerOneText;
    public TMPro.TextMeshProUGUI PlayerTwoText;

    private List<Player> playerList = new List<Player>();
    private int currentPlayerIndex = 0;

    public Player topPlayer;  // Player on the left side
    public Player bottomPlayer; // Player on the right side
    public bool isWaitingForRelease = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            touchManager = FindFirstObjectByType<TouchManager>();
            bombMechanic = FindFirstObjectByType<BombMechanic>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePlayers();
        StartFirstRound();
    }

    private void InitializePlayers()
    {
        playerList.Add(new Player("Juan"));
        playerList.Add(new Player("Paco"));
        playerList.Add(new Player("Carlota"));
        playerList.Add(new Player("Pilar"));
        ShufflePlayers();

        // Initialize first player immediately
        topPlayer = playerList[0];
        currentPlayerIndex = 1;
        UpdateUI();
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
        PlayerOneText.text = topPlayer?.Name ?? playerList[0].Name;
        PlayerTwoText.text = bottomPlayer?.Name ?? "-";
    }

    private void StartFirstRound()
    {
        IsGameActive = false;
        topPlayer = playerList[0];
        bottomPlayer = null;
        isWaitingForRelease = false;
        currentPlayerIndex = 1;
        UpdateUI();
    }

    public void StartGame()
    {
        IsGameActive = true;
        bombMechanic.SetBombLive();
    }

    public void EndGame()
    {
        IsGameActive = false;
        topPlayer = null;
        bottomPlayer = null;
        isWaitingForRelease = false;
        ShufflePlayers();
        StartFirstRound();
    }

    public Player SelectNextPlayer()
    {
        if (playerList.Count == 0) return null;

        Player next = playerList[currentPlayerIndex];
        currentPlayerIndex = (currentPlayerIndex + 1) % playerList.Count;
        Debug.Log("next player:" + next.Name);
        return next;
    }

    public void BombExploded()
    {
        Debug.Log($"BOOM! Game Over!");
        EndGame();
    }
}