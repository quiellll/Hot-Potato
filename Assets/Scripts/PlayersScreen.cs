using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersScreen : MonoBehaviour
{
    [SerializeField] private List<Image> playerImage;
    [SerializeField] private GameConfiguration config;

    public void UpdatePlaceholders()
    {
        for (int i = 0; i < playerImage.Count; i++)
        {
            var placeholder = playerImage[i];

            if (i < config.players.Count)
            {
                var player = config.players[i];
                placeholder.sprite = Sprite.Create(player.PlayerFace, new Rect(0.0f, 0.0f, player.PlayerFace.width, player.PlayerFace.height), new Vector2(0.5f, 0.5f), 100.0f);
                placeholder.GetComponentInParent<GameObject>().SetActive(true);
            }
            else
            {
                // Reset placeholder
                placeholder= null;
                placeholder.GetComponentInParent<GameObject>().SetActive(false);
            }
        }
    }
}