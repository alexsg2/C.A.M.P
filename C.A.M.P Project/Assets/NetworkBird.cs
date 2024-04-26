using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkBird : NetworkBehaviour
{

    public override void OnNetworkSpawn() {
        if (IsClient) {
            GetComponent<lb_BirdController>().enabled = false;
        }
    }

}
