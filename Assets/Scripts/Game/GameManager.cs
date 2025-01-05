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
        // Mecánicas de cuando empiece el juego [NO ESTOY USANDO ESTO RN]
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
        // aquí se pone la lógica para pillar al proximo jugador
        // importante que desde el menú paséis una lista de jugadores
        Debug.Log("Selecting next player...");
        // TODO: assign to currentPlayer
    }

    public void BombExploded()
    {
        // aquí todo lo que quieras que pase cuando explote.
        Debug.Log($"BOOM! Players lose: {CurrentPlayer}");
        EndGame();
    }
}
