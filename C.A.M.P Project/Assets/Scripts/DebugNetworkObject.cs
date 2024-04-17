using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DebugNetworkObject : NetworkBehaviour
{
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        Debug.Log("I WAS SPAWNED BABY LETS GOOO");
    }
}
