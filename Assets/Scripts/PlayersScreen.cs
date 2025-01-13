using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersScreen : MonoBehaviour
{
    [SerializeField] private List<RawImage> playerPlaceholders;
    [SerializeField] private List<Image> playerBackgrounds;
    [SerializeField] private GameConfiguration config;

    public void UpdatePlaceholders()
    {
        for (int i = 0; i < playerPlaceholders.Count; i++)
        {
            var placeholder = playerPlaceholders[i];
            var background = playerBackgrounds?[i];

            if (i < config.players.Count)
            {
                var player = config.players[i];
                // Update face - now using the PlayerFace property which handles conversion
                placeholder.texture = player.PlayerFace;
                placeholder.gameObject.SetActive(true);

                // Update background if available
                if (background != null && player.Background != null)
                {
                    background.sprite = player.Background;
                    background.gameObject.SetActive(true);
                }
            }
            else
            {
                // Reset placeholder
                placeholder.texture = null;
                placeholder.gameObject.SetActive(true);

                if (background != null)
                {
                    background.sprite = null;
                    background.gameObject.SetActive(false);
                }
            }
        }
    }
}