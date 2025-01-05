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

        // Start game when first player touches the screen
        if (!GameManager.Instance.IsGameActive && Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2)
            {
                GameManager.Instance.StartGame();
            }
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

        // Update visual indicators if you have them
        if (playerOneIndicator) playerOneIndicator.SetActive(isPlayerOneTouching);
        if (playerTwoIndicator) playerTwoIndicator.SetActive(isPlayerTwoTouching);
    }

    private void HandleTouchBegan(Touch touch)
    {
        GameManager gm = GameManager.Instance;
        bool touchingLeft = touch.position.x < Screen.width / 2;

        if (!gm.isWaitingForRelease)
        {
            // Second player touches right side
            if (!touchingLeft && gm.bottomPlayer == null)
            {
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

        if (gm.isWaitingForRelease && touchingLeft)
        {
            // Check if there are no other touches on the left side
            bool noOtherLeftTouches = true;
            foreach (Touch t in Input.touches)
            {
                if (t.fingerId != touch.fingerId && t.position.x < Screen.width / 2)
                {
                    noOtherLeftTouches = false;
                    break;
                }
            }

            if (noOtherLeftTouches)
            {
                // Left player released, update for next player
                Debug.Log($"Left player ({gm.topPlayer.Name}) released.");
                gm.topPlayer = gm.bottomPlayer;
                gm.bottomPlayer = null;
                gm.isWaitingForRelease = false;
                gm.UpdateUI();
            }
        }
    }
}