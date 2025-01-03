using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public GameObject playerOneIndicator;
    public GameObject playerTwoIndicator;

    private bool isPlayerOneTouching = false;
    private bool isPlayerTwoTouching = false;

    public void Update()
    {
        HandleTouchInput();

        if (!GameManager.Instance.IsGameActive && Input.touchCount > 0)
        {
            GameManager.Instance.StartGame();
        }

        if (GameManager.Instance.IsGameActive)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    HandleTouchBegan(touch);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    HandleTouchEnded(touch);
                }
            }
        }
    }

    private void HandleTouchInput()
    {
        isPlayerOneTouching = false;
        isPlayerTwoTouching = false;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    isPlayerOneTouching = true;
                }
                else
                {
                    isPlayerTwoTouching = true;
                }
            }
        }
    }

    private void HandleTouchBegan(Touch touch)
    {
        GameManager gm = GameManager.Instance;

        if (!gm.isWaitingForRelease)
        {
            bool touchingLeft = touch.position.x < Screen.width / 2;

            if (!touchingLeft && gm.bottomPlayer == null)
            {
                // Second player touches right side
                gm.bottomPlayer = gm.SelectNextPlayer();
                gm.isWaitingForRelease = true;
                Debug.Log($"Right player ({gm.bottomPlayer.Name}) touched. Waiting for left player release.");
                gm.UpdateUI();
            }
        }
    }

    private void HandleTouchEnded(Touch touch)
    {
        GameManager gm = GameManager.Instance;

        bool touchingLeft = touch.position.x < Screen.width / 2;

        if (gm.isWaitingForRelease && touchingLeft && !isPlayerOneTouching)
        {
            // Left player released, only update left player
            Debug.Log($"Left player ({gm.topPlayer.Name}) released.");
            gm.topPlayer = gm.SelectNextPlayer();
            gm.isWaitingForRelease = false;
            gm.UpdateUI();

            // Right player stays in place
            Debug.Log($"Right player ({gm.bottomPlayer.Name}) maintains position.");
        }
    }
}