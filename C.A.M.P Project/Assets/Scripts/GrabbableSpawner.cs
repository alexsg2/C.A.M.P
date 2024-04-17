using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GrabbableSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    private int maxToSpawn = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (IsServer || IsHost) {
            Debug.Log("Entered if yay");
            for (int i = 0; i < maxToSpawn; i++) {
                GameObject obj = Instantiate(prefabs[0], Vector3.zero, Quaternion.identity);
                // optional TODO: change transform position
                NetworkObject no = obj.GetComponent<NetworkObject>();
                if (no != null) {
                    no.Spawn();
                }
                else {
                    Debug.Log($"Prefab {i} has no NetworkObject component. Try adding one so we can spawn it.");
                }
            }
        }
        Debug.Log("Did not enter if statement");
    }
}
