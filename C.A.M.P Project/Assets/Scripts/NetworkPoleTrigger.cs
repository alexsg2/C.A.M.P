using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPoleTrigger : NetworkBehaviour
{

    [SerializeField]
    private NetworkTentTask.Side side;
    public GameObject pole;
    public NetworkTentTask tentTask;

    // on network spawn: add listeners

    //  react to collider only as server --> update tentTask.poles(left | right)
    // everyone responds to this event and sets pole active if they match left right whatever and pole isnt already enabled
}
