using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public GameObject playerOneIndicator;
    public GameObject playerTwoIndicator;

    public bool isPlayerOneTouching = false;
    public bool isPlayerTwoTouching = false;

    public void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        HandleTouchInput();
        UpdateIndicators();
    }

    private void HandleTouchInput()
    {
        isPlayerOneTouching = false;
        isPlayerTwoTouching = false;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
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
        // debug states
        Debug.Log($"Player One Touching: {isPlayerOneTouching}, Player Two Touching: {isPlayerTwoTouching}");
    }

    private void UpdateIndicators()
    {
        playerOneIndicator.SetActive(isPlayerOneTouching);
        playerTwoIndicator.SetActive(isPlayerTwoTouching);
    }
}
