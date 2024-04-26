using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


/// <summary>
/// Disable move script on client.
/// </summary>
public class NetworkNPCMovement : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient) {
            Moving move_script = GetComponent<Moving>();
            move_script.enabled = false;
        }
    }
}
