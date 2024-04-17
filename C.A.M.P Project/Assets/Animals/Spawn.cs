using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<SpawnSettings> animals = new List<SpawnSettings>();

    private Terrain terrain;

    private void Start()
    {
        terrain = Terrain.activeTerrain;
        
        if (terrain) {
            SpawnAnimals();
        }
    }

    private void SpawnAnimals()
    {
        foreach (var spawnSettings in animals)
        {
            for (int i = 0; i < spawnSettings.spawnCount; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition(spawnSettings);
                Instantiate(spawnSettings.spawnPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition(SpawnSettings spawnSettings) {

        // if (spawnSettings.spawnPrefab.tag == "Rabbit") {
        //     float x = -2.890247f;
        //     float z = -13.32985f;
        //     float y = terrain.SampleHeight(new Vector3(x, -2.5f, z));
            
        //     return new Vector3(x, y, z);
        // }

        if (spawnSettings.spawnPrefab.tag == "Frog") {
            float x = -15f;
            float z = -9f;
            float y = terrain.SampleHeight(new Vector3(x, -2.5f, z));
            
            return new Vector3(x, y, z);
        }

        if (spawnSettings.spawnPrefab.tag == "Deer") {
            float x = 16f;
            float z = 17.5f;
            float y = terrain.SampleHeight(new Vector3(x, -2.5f, z));
            
            return new Vector3(x, y, z);
        }

        return Vector3.zero;
    }
}

[System.Serializable]

public class SpawnSettings
{
    public GameObject spawnPrefab;
    public int spawnCount = 10;
}