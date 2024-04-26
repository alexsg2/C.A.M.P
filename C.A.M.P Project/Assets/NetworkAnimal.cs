using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkAnimal : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient) {
            Animal animal = GetComponent<Animal>();
            animal.enabled = false;
        }
    }
}
