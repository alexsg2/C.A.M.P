using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPoleTrigger : NetworkBehaviour
{
    public Side side;
    public GameObject pole;
    public GameObject indicator;
    public NetworkTentTask tentTask;

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        if (IsClient) { // client no run anything
            Destroy(this.gameObject);
        }

        Debug.Log($"NetworkPole {side} spawned");

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handle when pole enters trigger, update poles netvar, disable this 
    /// game object.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // only server manages state change
        // trigger enter doesnt matter if pole is already activated
        if (pole.activeSelf) {
            return;
        }

        if (other.CompareTag("Pole"))
        {
            // despawn/destroy pole
            GameObject go = other.gameObject;
            NetworkObject no = go.GetComponent<NetworkObject>();
            if (no != null) { // server despawn it
                if (!no.IsOwnedByServer) {
                    no.RemoveOwnership();
                }
                no.Despawn();
                Debug.Log($"TENT: Despawned pole grabbable for {side} pole trigger");
            }
            // else { // everyone destroy it
            //     Destroy(go);
            //     Debug.Log($"TENT: Destroyed pole grabbable for {side} pole trigger");
            // }

            // update netvar for state
            TwoBools curr = tentTask.poles.Value;

            if (side == Side.left) {
                curr.left = true;
            }
            else {
                curr.right = true;
            }

            tentTask.poles.Value = curr;


            // indicator.SetActive(false);
            // Disable this trigger, it's been used
            gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
