using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


/// <summary>
/// Disable move script on client.
/// </summary>
public class NetworkNPCMovement : NetworkBehaviour
{
    public Canvas dialogue_canvas; 
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient) {
            Moving move_script = GetComponent<Moving>();
            move_script.enabled = false;

            // TODO: get local player object and camera, set canvas camera
        }

    }
}
