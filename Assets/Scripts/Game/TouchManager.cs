using UnityEngine;
using System.Collections.Generic;
using System;

public class TouchManager : MonoBehaviour
{
    // Events that other components can subscribe to
    public event Action<PlayerPosition> OnTouchBegan;
    public event Action<PlayerPosition> OnTouchEnded;

    private Dictionary<int, PlayerPosition> activeTouches = new Dictionary<int, PlayerPosition>();

    public bool IsTopTouched => GetTouchCount(PlayerPosition.Top) > 0;
    public bool IsBottomTouched => GetTouchCount(PlayerPosition.Bottom) > 0;

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            // Determine if touch is in top or bottom half of screen
            PlayerPosition position = touch.position.y > Screen.height / 2 ?
                PlayerPosition.Top : PlayerPosition.Bottom;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    activeTouches[touch.fingerId] = position;
                    OnTouchBegan?.Invoke(position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (activeTouches.Remove(touch.fingerId))
                    {
                        OnTouchEnded?.Invoke(position);
                    }
                    break;
            }
        }
    }

    private int GetTouchCount(PlayerPosition position)
    {
        int count = 0;
        foreach (var touch in activeTouches.Values)
        {
            if (touch == position) count++;
        }
        return count;
    }

    // Helper method to check if both areas are being touched simultaneously
    public bool IsDuringHandoff()
    {
        return IsTopTouched && IsBottomTouched;
    }
}