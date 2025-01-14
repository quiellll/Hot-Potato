using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public Sprite Background;
    public Sprite PlayerFace;

    private Vector2 imageResolution;

    public PlayerData(string name, Sprite background, Sprite face)
    {
        Name = name;
        Background = background;
        PlayerFace = face;
    }

    public void SetImageResolution(Vector2 imageResolution)
    {
        this.imageResolution = imageResolution;
    }

    public Vector2 GetImageResolution()
    {
        return imageResolution;
    }
}