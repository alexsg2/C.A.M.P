using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkNailTrigger : NetworkBehaviour
{
    public GameObject nail; // nail to enable / move
    public GameObject placement_indicator; // indicator we should disable when done

    public Count count; // what nail is this? 1, 2, 3, or 4

    public NetworkTentTask tentTask; // tent task script with useful netvar we need to update

    // number of current nail hits
    private NetworkVariable<int> num_hits = new NetworkVariable<int>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    // was the nail triggered? can we start hitting it?
    private NetworkVariable<bool> nail_active = new NetworkVariable<bool>(
        false, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    [SerializeField]
    private const int hits_needed = 2; // # of total hits we need for nail

    // private int currNailHits = 0; // flag to track number of nail hits
    // private int neededNailHits = 2; // constant to track number of needed nail hits

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // listen for nail trigger, networktenttask disables this object by default
        // then enables it when needed
        nail_active.OnValueChanged += OnNailTrigger;
    }

    public void OnNailTrigger(bool old, bool updated) {
        if (!updated) {
            return;
        }
        // enable nail in scene
        nail.SetActive(true);
        // listen 4 number of hits now
        num_hits.OnValueChanged += OnNailHitsChange;
        nail_active.OnValueChanged -= OnNailTrigger;
        Debug.Log($"Nail {count} has been installed, listening for hammer hits");
    }


    public void OnNailHitsChange(int old, int updated) {
        // Move the nail down slightly
        nail.transform.position -= Vector3.up * 0.08f;
        Debug.Log($"Hit nail {count}! Total hits: {updated}");

        if (updated >= hits_needed) {
            // stop listening to this event
            num_hits.OnValueChanged -= OnNailHitsChange;

            // update status of this subtask!
            if (IsServer) {
                FourBools curr = tentTask.nails.Value;
                switch (count) {
                    case Count.one:
                        curr.one = true;
                        break;
                    case Count.two:
                        curr.two = true;
                        break;
                    case Count.three:
                        curr.three = true;
                        break;
                    case Count.four:
                        curr.four = true;
                        break;
                }
                tentTask.nails.Value = curr;
                Debug.Log($"Nail {count} is fully hammered in!");
            }
            
            // disable indicator
            placement_indicator.SetActive(false);
            // disable this trigger, it's served its purpose
            this.gameObject.SetActive(false);
        }
    }

    // Two state changes: 1. getting nail 2. Hammering nail
    private void OnTriggerEnter(Collider other)
    {
        // only server handles updating state network vars
        if (IsClient) {
            return;
        }

        // handle when we get a nail
        if (other.CompareTag("Nail") && !nail_active.Value)
        {
            // destroy nail
            GameObject go = other.gameObject;
            NetworkObject no = go.GetComponent<NetworkObject>();
            if (no != null) {
                if (!no.IsOwnedByServer) {
                    no.RemoveOwnership();
                }
                no.Despawn();
                Debug.Log($"TENT: Despawned nail grabbable for {count} nail trigger");
            }
            else {
                Destroy(go);
                Debug.Log($"TENT: Destroyed nail grabbable for {count} nail trigger");
            }
            nail_active.Value = true;
            return;
        }

        // handle hammering in nail
        if (other.CompareTag("Hammer") && (num_hits.Value < hits_needed))
        {
            // Increment hit counter
            num_hits.Value += 1;
        }
    }
}
