using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject toSpawn;

    public void Start() {
        NetworkManager.Singleton.OnServerStarted += OnServerStart;
    }

    public void OnServerStart() {
        if (toSpawn == null) {
            return;
        }

        var instance = Instantiate(toSpawn);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();

        if (instanceNetworkObject == null) {
            return;
        }
        
        instanceNetworkObject.Spawn();
    }
}
