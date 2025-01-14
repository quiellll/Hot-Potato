using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersScreen : MonoBehaviour
{
    [SerializeField] private List<Image> playerImage;
    [SerializeField] private GameConfiguration config;

    private void OnEnable()
    {
        UpdatePlaceholders();
    }

    public void UpdatePlaceholders()
    {
        for (int i = 0; i < playerImage.Count; i++)
        {
            if (playerImage[i] == null) continue;

            var imageComponent = playerImage[i];
            var imageParent = imageComponent.transform.parent.gameObject;

            if (i < config.players.Count && config.players[i] != null)
            {
                var player = config.players[i];

                if (player.PlayerFace != null)
                {
                    imageComponent.sprite = Sprite.Create(
                        player.PlayerFace,
                        new Rect(0, 0, player.PlayerFace.width, player.PlayerFace.height),
                        new Vector2(0.5f, 0.5f),
                        100.0f
                    );

                    imageComponent.enabled = true;
                    Color color = imageComponent.color;
                    color.a = 1f;
                    imageComponent.color = color;
                }
            }
            else
            {
                imageComponent.sprite = null;

                if (imageParent != null)
                {
                    imageParent.SetActive(true);
                }
            }
        }
    }
}