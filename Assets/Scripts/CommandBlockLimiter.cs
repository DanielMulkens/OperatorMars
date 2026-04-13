using UnityEngine;
using System.Collections.Generic;

public class BlockSpawnManager : MonoBehaviour
{
    [Header("Block Settings")]
    public GameObject[] blockPrefabs;
    public int maxBlocksPerPrefab = 10;

    private Dictionary<GameObject, Queue<GameObject>> spawnedBlocks 
        = new Dictionary<GameObject, Queue<GameObject>>();

    public bool TrySpawnBlock(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!spawnedBlocks.ContainsKey(prefab))
            spawnedBlocks[prefab] = new Queue<GameObject>();

        if (spawnedBlocks[prefab].Count >= maxBlocksPerPrefab)
            return false;

        GameObject newBlock = Instantiate(prefab, position, rotation);
        spawnedBlocks[prefab].Enqueue(newBlock);
        return true;
    }

    public int GetBlockCount(GameObject prefab)
    {
        return spawnedBlocks.ContainsKey(prefab) ? spawnedBlocks[prefab].Count : 0;
    }
}