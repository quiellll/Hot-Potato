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
    [SerializeField] private GameObject playerNameCardLayoutParent;
    [SerializeField] private GameObject playerNameCard;

    private void OnEnable()
    {
        UpdateStartButtonVisibility();
        //UpdateNamesVisibility();
    }

    void Start()
    {
        UpdateStartButtonVisibility();
        UpdateNamesVisibility();
    }

    private void UpdateStartButtonVisibility()
    {
        if (StartButton != null)
        {
            bool hasEnoughPlayers = config.players.Count >= minimumPlayers;

            // Keep button visible but change interactability
            StartButton.interactable = hasEnoughPlayers;

            //// Update message text
            //if (messageText != null)
            //{
            //    if (!hasEnoughPlayers)
            //    {
            //        messageText.text = $"Need at least {minimumPlayers} players to start";
            //        messageText.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        messageText.text = "";
            //        messageText.gameObject.SetActive(false);
            //    }
            //}
        }
    }

    public void UpdateNamesVisibility()
    {
        foreach (var player in config.players)
        {
            var newCard = Instantiate(playerNameCard);
            newCard.GetComponentInChildren<TextMeshProUGUI>().text = player.Name;
            newCard.transform.SetParent(playerNameCardLayoutParent.transform, false);
        }
    }
    
    public void RefreshStartButton()
    {
        UpdateStartButtonVisibility();
    }
}