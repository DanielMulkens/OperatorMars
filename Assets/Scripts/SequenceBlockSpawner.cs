using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;          // Empty GameObject as spawn location
    public GameObject[] prefabsToSpawn;   // Drag & drop multiple prefabs here

    /// <summary>
    /// Spawn a prefab by index in the prefabsToSpawn array
    /// </summary>
    /// <param name="index">Index of the prefab to spawn</param>
    public void SpawnPrefabByIndex(int index)
    {
        if (index < 0 || index >= prefabsToSpawn.Length)
        {
            Debug.LogWarning("Prefab index out of range!");
            return;
        }

        GameObject prefab = prefabsToSpawn[index];
        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}