using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MobSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject mobPrefab;
    public int maxObjects = 3;
    public float minDistance = 3f;

    private List<Vector3> validSpawnPos = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameController.OnLevelReset += SpawnMob;
        SpawnMob();
    }
    private void OnDestroy()
    {
        GameController.OnLevelReset -= SpawnMob;
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
        foreach(GameObject mob in spawnObjects)
        {
            if (mob == null) continue;

            float distance = Vector3.Distance(position, mob.transform.position);

            if (distance < minDistance) return false;
        }
        return true;
    }
    private void SpawnMob()
    {
        ClearMob();

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

            GameObject mob = Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
            spawnObjects.Add(mob);

        }
    }
    private void ClearMob()
    {
        foreach (GameObject mob in spawnObjects)
        {
            if(mob != null)
            {
                Destroy(mob);
            }
        }
        spawnObjects.Clear();
    }
}
