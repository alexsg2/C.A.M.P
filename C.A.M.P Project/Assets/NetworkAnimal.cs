using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkAnimal : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsHost && !IsServer) {
            Animal animal = GetComponent<Animal>();
            animal.enabled = false;
        }
    }
}
