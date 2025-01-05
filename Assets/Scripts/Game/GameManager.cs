using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameActive { get; private set; } = true;
    public string CurrentPlayer;


    public TouchManager touchManager;
    public BombMechanic bombMechanic;

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

    public void StartGame()
    {
        // Mec�nicas de cuando empiece el juego [NO ESTOY USANDO ESTO RN]
        IsGameActive = true;
        bombMechanic.SetBombLive();
        Debug.Log("Game started!");
    }

    public void EndGame()
    {
        IsGameActive = false;
        // la bomba se desactiva en el bombmechanic
        Debug.Log("Game ended!");
    }

    public void SelectNextPlayer()
    {
        // aqu� se pone la l�gica para pillar al proximo jugador
        // importante que desde el men� pas�is una lista de jugadores
        Debug.Log("Selecting next player...");
        // TODO: assign to currentPlayer
    }

    public void BombExploded()
    {
        // aqu� todo lo que quieras que pase cuando explote.
        Debug.Log($"BOOM! Players lose: {CurrentPlayer}");
        EndGame();
    }
}
