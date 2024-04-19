using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DebugNetworkObject : NetworkBehaviour
{
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        Debug.Log("I was spawned");
    }

    public void Awake() {
        Debug.Log("I am awake");
    }

    public void Start() {
        Debug.Log("Started");
    }
}
