using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPoleTrigger : NetworkBehaviour
{
    public Side side;
    public GameObject pole;
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
        if (IsClient || pole.activeSelf) {
            return;
        }

        if (other.CompareTag("Pole"))
        {

            // despawn/destroy pole
            GameObject go = other.gameObject;
            NetworkObject no = go.GetComponent<NetworkObject>();
            if (no != null) {
                if (!no.IsOwnedByServer) {
                    no.RemoveOwnership();
                }
                no.Despawn();
                Debug.Log($"TENT: Despawned pole grabbable for {side} pole trigger");
            }
            else {
                Destroy(go);
                Debug.Log($"TENT: Destroyed pole grabbable for {side} pole trigger");
            }

            // update netvar for state
            TwoBools curr = tentTask.poles.Value;

            if (side == Side.left) {
                curr.left = true;
            }
            else {
                curr.right = true;
            }

            tentTask.poles.Value = curr;

            // Disable this trigger, it's been used
            gameObject.SetActive(false);
        }
    }
}