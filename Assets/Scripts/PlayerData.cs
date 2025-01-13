using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string Name;
    public Sprite Background;
    public Texture2D PlayerFace; // Cache the decoded texture

    public PlayerData(string name, Sprite background, Texture2D face)
    {
        Name = name;
        Background = background;
        PlayerFace = face;
    }
}