using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Level Info")]
    public string levelName = "Level";       // Friendly name of the level

    [Header("Gameplay Settings")]
    public int maxSequenceSends = 1;         // How many times the player can send the sequence
    public Transform robotStart;             // Where the robot should spawn
    public Collider targetArea;              // Completion area
}