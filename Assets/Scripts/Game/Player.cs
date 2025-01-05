using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/Player")]
public class Player: ScriptableObject
{
    public string Name;
    public Image Background;
    public Image PlayerFace;



    public Player(string name, Image background, Image face)
    { 
        Name = name; 
        Background= background;
        PlayerFace = face;
    }

}
