using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GemSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject gemPrefab;
    public int maxObjects = 10;
    public float minDistance = 2f;

    private List<Vector3> validSpawnPos = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameController.OnLevelReset += SpawnGem;
        SpawnGem();
    }
    private void OnDestroy()
    {
        GameController.OnLevelReset -= SpawnGem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CheckValidPos()
    {
        validSpawnPos.Clear();

        BoundsInt bounds = tilemap.cellBounds;

        for(int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for(int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int groundCell = new Vector3Int(x, y, 0);
                Vector3Int spawnCell = new Vector3Int(x, y + 1, 0);

                if (!tilemap.HasTile(groundCell))
                    continue;

                if (tilemap.HasTile(spawnCell))
                    continue;

                Vector3 spawnPosition = tilemap.GetCellCenterWorld(spawnCell);
                validSpawnPos.Add(spawnPosition);
            }
        }
    }
    private bool CheckDistance(Vector3 position)
    {
        foreach(GameObject gem in spawnObjects)
        {
            if (gem == null) continue;

            float distance = Vector3.Distance(position, gem.transform.position);

            if (distance < minDistance) return false;
        }
        return true;
    }
    private void SpawnGem()
    {
        ClearGem();

        CheckValidPos();

        if(validSpawnPos.Count == 0)
        {
            return;
        }

        int maxTry = validSpawnPos.Count * 2;
        int tryCount = 0;

        while(spawnObjects.Count < maxObjects && validSpawnPos.Count > 0 && tryCount < maxTry)
        {
            tryCount++;

            int randomIndex = Random.Range(0, validSpawnPos.Count);
            Vector3 spawnPosition = validSpawnPos[randomIndex];
            validSpawnPos.RemoveAt(randomIndex);

            if (!CheckDistance(spawnPosition)) continue;

            GameObject gem = Instantiate(gemPrefab, spawnPosition, Quaternion.identity);
            spawnObjects.Add(gem);

        }
    }
    private void ClearGem()
    {
        foreach (GameObject gem in spawnObjects)
        {
            if(gem != null)
            {
                Destroy(gem);
            }
        }
        spawnObjects.Clear();
    }
}
