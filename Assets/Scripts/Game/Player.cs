using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Basic player properties
    public string Name { get; private set; }
    public Sprite Background { get; private set; }
    public Texture PlayerFace { get; private set; }
    public bool IsEliminated { get; private set; }
    public PlayerPosition CurrentPosition { get; private set; }

    public Player(string name, Sprite background, Texture face)
    {
        Name = name;
        Background = background;
        PlayerFace = face;
        IsEliminated = false;
    }
    public void UpdatePosition(PlayerPosition newPosition)
    {
        CurrentPosition = newPosition;
        UpdateVisuals();
    }

    public void Eliminate()
    {
        IsEliminated = true;
        UpdateVisuals();
    }

    public void Reset()
    {
        IsEliminated = false;
        UpdateVisuals();
    }

    // Updates player visuals based on their current state
    private void UpdateVisuals()
    {
        //if (Background != null)
        //{
        //    Background.color = IsEliminated ? Color.gray : Color.white;
        //}

        //if (PlayerFace != null)
        //{
        //    PlayerFace.gameObject.SetActive(!IsEliminated);
        //}
    }
}