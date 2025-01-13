using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/Configuration")]
public class GameConfiguration : ScriptableObject
{
    public List<PlayerData> players = new List<PlayerData>();
}