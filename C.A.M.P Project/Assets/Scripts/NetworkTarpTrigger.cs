using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkTarpTrigger : NetworkBehaviour
{
    public Side side;
    public GameObject tarp;
    public NetworkTentTask tentTask;

    /// <summary>
    /// Handle when pole enters trigger, update poles netvar, disable this 
    /// game object.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // only server manages state change
        // trigger enter doesnt matter if pole is already activated
        if (IsClient || tarp.activeSelf) {
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
            else {
                Destroy(go);
                Debug.Log($"TENT: Destroyed tarp grabbable for {side} tarp trigger");
            }

            // update netvar for state
            TwoBools curr = tentTask.tarps.Value;

            if (side == Side.left) {
                curr.left = true;
            }
            else {
                curr.right = true;
            }

            tentTask.tarps.Value = curr;

            // Disable this trigger, it's been used
            gameObject.SetActive(false);
        }
    }
}
