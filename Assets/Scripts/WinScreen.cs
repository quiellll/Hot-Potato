using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI winnerNameText;
    [SerializeField] private Image playerFaceImage;
    [SerializeField] private Image backgroundImage;

    void Start()
    {
        // Get the winner data from GameManager
        if (GameManager.Instance != null)
        {
            Player winner = GameManager.Instance.GetWinner();
            if (winner != null)
            {
                // Set up the UI elements
                winnerNameText.text = $"{winner.Name}";
                if (winner.PlayerFace != null)
                    playerFaceImage.sprite = winner.PlayerFace; ;
                if (winner.Background != null)
                    backgroundImage.sprite = winner.Background;
            }
        }
    }

    public void UpdateWinScreen()
    {
        if (GameManager.Instance != null)
        {
            Player winner = GameManager.Instance.GetWinner();
            if (winner != null)
            {
                winnerNameText.text = $"{winner.Name}";
                if (winner.PlayerFace != null)
                    playerFaceImage.sprite = winner.PlayerFace;
                if (winner.Background != null)
                    backgroundImage.sprite = winner.Background;
            }
        }
    }

}