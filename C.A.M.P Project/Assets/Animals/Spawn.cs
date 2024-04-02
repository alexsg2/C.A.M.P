using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<SpawnSettings> animals = new List<SpawnSettings>();

    private Terrain terrain;
    private float terrainWidth;
    private float terrainLength;

    private void Start()
    {
        terrain = Terrain.activeTerrain;
        
        if (terrain) {
            terrainWidth = terrain.terrainData.size.x;
            terrainLength = terrain.terrainData.size.z;
        
            SpawnAnimals();
        }
    }

    private void SpawnAnimals()
    {
        foreach (var spawnSettings in animals)
        {
            for (int i = 0; i < spawnSettings.spawnCount; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                Instantiate(spawnSettings.spawnPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition() {
        float x = Random.Range(0f, terrainWidth);
        float z = Random.Range(0f, terrainLength);
        float y = terrain.SampleHeight(new Vector3(x, 0, z));

        return new Vector3(x, y, z);
    }
}

[System.Serializable]

public class SpawnSettings
{
    public GameObject spawnPrefab;
    public int spawnCount = 10;
}