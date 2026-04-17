using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Level Info")]
    public string levelName = "Level";

    [Header("Narrative")]
    [TextArea(3, 8)]
    public string narrativeText = "";

    [Header("Gameplay Settings")]
    public int maxSequenceSends = 1;
    public Transform robotStart;
    public Collider targetArea;
}