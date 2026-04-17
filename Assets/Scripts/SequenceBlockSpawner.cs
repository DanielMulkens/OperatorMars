using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public GameObject[] prefabsToSpawn;

    [Header("Limit Settings")]
    public int maxPerPrefab = 5;

    private Dictionary<int, List<GameObject>> spawnedBlocks = new Dictionary<int, List<GameObject>>();

    public void SpawnPrefabByIndex(int index)
    {
        if (index < 0 || index >= prefabsToSpawn.Length)
        {
            Debug.LogWarning("Prefab index out of range!");
            return;
        }

        // Initialize list for this index if needed
        if (!spawnedBlocks.ContainsKey(index))
            spawnedBlocks[index] = new List<GameObject>();

        // Remove any destroyed blocks from the list
        spawnedBlocks[index].RemoveAll(b => b == null);

        // Check limit
        if (spawnedBlocks[index].Count >= maxPerPrefab)
        {
            Debug.Log($"Max blocks reached for prefab {index}!");
            return;
        }

        GameObject prefab = prefabsToSpawn[index];
        if (prefab != null && spawnPoint != null)
        {
            GameObject newBlock = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            spawnedBlocks[index].Add(newBlock);
        }
    }
}