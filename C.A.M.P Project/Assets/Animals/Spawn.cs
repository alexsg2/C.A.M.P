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

        float x = Random.Range(-10f, 15f);
        float z = Random.Range(-10f, 15f);
        float y;

        if (spawnSettings.spawnPrefab.tag == "Bird") {
            x = Random.Range(-60f, 60f);
            z = Random.Range(-60f, 60f);
            y = terrain.SampleHeight(new Vector3(x, -2.5f, z)) + 50f; // If the animal is a Bird, we want to spawn it in the sky (y = 50)
        } else {
            y = terrain.SampleHeight(new Vector3(x, -2.5f, z)); // If the animal is a Rabbit (or any other land animal), we want to spawn it on the terrain (y = 0)
        }

        return new Vector3(x, y, z);
    }
}

[System.Serializable]

public class SpawnSettings
{
    public GameObject spawnPrefab;
    public int spawnCount = 10;
}