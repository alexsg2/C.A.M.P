using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
    public GameObject obj;

    [SerializeField]
    private int amountToSpawn;

    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f, 10.0f);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer || IsHost && amountToSpawn > 0) {
            for (int i = 0; i < amountToSpawn; i++) {
                Spawn();
            }
        }
    }

    private void Spawn() {
        GameObject go = Instantiate(obj, Vector3.zero, Quaternion.identity);
        // float y = transform.position.y;
        go.transform.position = new Vector3(
            transform.position.x + Random.Range(placementArea.x, placementArea.y), 
            transform.position.y, 
            transform.position.z + Random.Range(placementArea.x, placementArea.y));
        Quaternion new_rotation = Quaternion.Euler(go.transform.rotation.x, Random.Range(0, 360), go.transform.rotation.z);
        go.transform.rotation = new_rotation;
        go.GetComponent<NetworkObject>().Spawn();
    }
}
