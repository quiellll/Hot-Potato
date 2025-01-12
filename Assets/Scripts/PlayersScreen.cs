using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersScreen : MonoBehaviour 
{
    [SerializeField] private List<RawImage> playerPlaceholders; // list of placeholder images
    [SerializeField] private GameConfiguration config;

    private void OnEnable()
    {
        UpdatePlaceholders();
    }

    public void UpdatePlaceholders()
    {
        for (int i = 0; i < playerPlaceholders.Count; i++)
        {
            if (i < config.players.Count)
            {
                // assign the player's face texture to the placeholder
                playerPlaceholders[i].texture = config.players[i].PlayerFace;
            }
            else
            {
                // reset placeholder if no player exists
                playerPlaceholders[i].texture = null;
            }
        }
    }
}
