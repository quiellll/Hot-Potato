using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayersScreen : MonoBehaviour
{
    [SerializeField] private List<Sprite> playerPlaceholders;
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
                placeholder = Sprite.Create(player.PlayerFace, new Rect(0.0f, 0.0f, player.PlayerFace.width, player.PlayerFace.height), new Vector2(0.5f, 0.5f), 100.0f);
                placeholder.GetComponentInParent<GameObject>().SetActive(true);

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
                placeholder= null;
                placeholder.GetComponentInParent<GameObject>().SetActive(false);

                if (background != null)
                {
                    background.sprite = null;
                    background.gameObject.SetActive(false);
                }
            }
        }
    }
}