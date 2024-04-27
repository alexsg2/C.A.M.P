using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTarpTrigger : NetworkBehaviour
{
    public Side side;
    public GameObject tarp;
    public GameObject indicator;
    public NetworkTentTask tentTask;

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        if (IsClient) {
            Destroy(this.gameObject);
        }

        Debug.Log($"Network Tarp trigger {side} spawned");
                if (IsClient) { // client no run anything
            gameObject.SetActive(false);
            this.enabled = false;
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handle when pole enters trigger, update poles netvar, disable this 
    /// game object.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // only executed by server
        // executed by server and client since we are destroying, not despawning
        // only server manages state change
        // trigger enter doesnt matter if pole is already activated
        if (tarp.activeSelf) {
            return;
        }

        if (other.CompareTag("Tarp"))
        {
            // despawn/destroy tarp
            GameObject go = other.gameObject;
            NetworkObject no = go.GetComponent<NetworkObject>();
            if (no != null) {
                if (!no.IsOwnedByServer) {
                    no.RemoveOwnership();
                }
                no.Despawn();
                Debug.Log($"TENT: Despawned tarp grabbable for {side} tarp trigger");
            }
            // else { // everyone destroy it
            //     Destroy(go);
            //     Debug.Log($"TENT: Destroyed tarp grabbable for {side} tarp trigger");
            // }
                // update netvar for state
            TwoBools curr = tentTask.tarps.Value;

            if (side == Side.left) {
                curr.left = true;
            }
            else {
                curr.right = true;
            }
            tentTask.tarps.Value = curr;

            // indicator.SetActive(false);
            // Disable this trigger, it's been used
            gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
