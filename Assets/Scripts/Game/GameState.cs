// Controls the game flow and valid actions at each moment
public enum GameState
{
    WaitingToStart,  // Initial state, waiting for first player
    Playing,         // Active gameplay, includes handoff transitions
    GameOver         // Round ended (either by explosion or timeout)
}