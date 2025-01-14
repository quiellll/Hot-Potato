using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayersScreen : MonoBehaviour
{
    [SerializeField] private List<Image> playerImage;
    [SerializeField] private GameConfiguration config;

    [SerializeField] private Button StartButton; // Reference to the start button

    [Header("Configuration")]
    [SerializeField] private int minimumPlayers = 3; // Minimum required players

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI messageText;
    private void OnEnable()
    {
        UpdatePlaceholders();
        UpdateStartButtonVisibility();
    }

    void Start()
    {
        UpdatePlaceholders();
        UpdateStartButtonVisibility();
    }
    private void UpdateStartButtonVisibility()
    {
        if (StartButton != null)
        {
            bool hasEnoughPlayers = config.players.Count >= minimumPlayers;

            // Keep button visible but change interactability
            StartButton.interactable = hasEnoughPlayers;

            // Update message text
            if (messageText != null)
            {
                if (!hasEnoughPlayers)
                {
                    messageText.text = $"Need at least {minimumPlayers} players to start";
                    messageText.gameObject.SetActive(true);
                }
                else
                {
                    messageText.text = "";
                    messageText.gameObject.SetActive(false);
                }
            }
        }
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
                    imageComponent.sprite = player.PlayerFace;

                    imageComponent.enabled = true;
                    Color color = imageComponent.color;
                    color.a = 1f;
                    imageComponent.color = color;
                }

                if (player.PlayerFace != null)
                {
                    imageComponent.sprite = player.PlayerFace;
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
        UpdateStartButtonVisibility();
    }
    public void RefreshStartButton()
    {
        UpdateStartButtonVisibility();
    }
}