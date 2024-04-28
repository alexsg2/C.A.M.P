using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;

public enum FireTaskStatus {
    Wait,
    Start,
    TwigsDone,
    Done

}

// Network Behavior for fire task. Handles variables for
// task like twig count and match count, as well as behavior
// like actually activating logs / fire.
public class NetworkFireTask : NetworkBehaviour
{
    // Objects to toggle as we go!
    public GameObject logstack; // Logs to toggle visibility
    public GameObject fire;
    public GameObject fireLight;

    public GameObject matchesIndicator;
    public GameObject fireIndicator;

    // cache instance ids of objects that are "in" collider
    // so we don't handle them again
    private HashSet<int> objects_inside = new HashSet<int>();

    // Network variables for task
    private NetworkVariable<int> twigsCount = new NetworkVariable<int>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
    private NetworkVariable<int> matchCount = new NetworkVariable<int>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    /// <summary>
    /// 0 = started, 1 = logs made, 2 = fire made (done)
    /// </summary>
    // public NetworkVariable<int> TaskStatus = new NetworkVariable<int>(
    //     0,  
    //     NetworkVariableReadPermission.Everyone, 
    //     NetworkVariableWritePermission.Server); 
    public NetworkVariable<FireTaskStatus> TaskStatus = new NetworkVariable<FireTaskStatus> (
        FireTaskStatus.Wait,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    [SerializeField]
    private int twigsRequired = 12; // Number of twigs required

    [SerializeField]
    private int matchesRequired = 3; // Number of matches required

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient && TaskStatus.Value > 0) {
            // let joining client catch up
            FastForward();
            TaskStatus.OnValueChanged += OnTaskStatusChange;
        }
        fireIndicator.SetActive(false);
        matchesIndicator.SetActive(false);
        TaskStatus.OnValueChanged += OnTaskStatusChange;
    }

    private void StartTask() {
        twigsCount.OnValueChanged += OnTwigCountChange;
        fireIndicator.SetActive(true);
    }

    private void StartMatches() {
        matchesIndicator.SetActive(true);

        twigsCount.OnValueChanged -= OnTwigCountChange;
        matchCount.OnValueChanged += OnMatchCountChange;
    }
    /// <summary>
    /// Fast forward a client's task status if the client just
    /// joined in the middle of completion.
    /// 
    /// Pre: at least know that status >= 1
    /// </summary>
    private void FastForward(){
        if (LogStackMade()) {
            MakeLogs();
        }
        
        if (FireMade()) {
            MakeFire();
        }
        else {
            // if fire is not made, add listeners
            matchCount.OnValueChanged += OnMatchCountChange;
            TaskStatus.OnValueChanged += OnTaskStatusChange;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        twigsCount.OnValueChanged -= OnTwigCountChange;
        TaskStatus.OnValueChanged -= OnTaskStatusChange;
    }

    // Debug log when twig count updates,
    // server checks if we met necessary twig amount
    // and updated Task1Status accordingly.
    public void OnTwigCountChange(int old, int updated) {
        // Debug.Log("Twigs count: " + updated);

        if (IsServer) {
            if (updated >= twigsRequired) {
                TaskStatus.Value = FireTaskStatus.TwigsDone;
            }
        }
    }

    // Debug log when match count updates,
    // server checks if we met necessary match amount
    // and updated Task1Status accordingly.
    public void OnMatchCountChange(int old, int updated) {
        // Debug.Log("Matches count: " + updated);

        if (IsServer) {
            if (updated >= matchesRequired) {
                TaskStatus.Value = FireTaskStatus.Done;
            }
        }
    }

    // Handle fire state when our task completion state
    // changes.
    public void OnTaskStatusChange(FireTaskStatus old, FireTaskStatus updated) {
        // both servers and clients execute this
        switch (updated) {
            case FireTaskStatus.Wait:
                // Debug.Log("Start firepit task. GET SOME TWIGS!");
                break;
            case FireTaskStatus.Start:
                StartTask();
                break;
            case FireTaskStatus.TwigsDone:
                // enable fire
                // deregister listener for match count
                MakeLogs();
                // Debug.Log("Enough twigs have been gathered, logs activated!");
                StartMatches();
                break;
            case FireTaskStatus.Done:
                MakeFire();
                // Debug.Log("Enough lit matches have been gathered, fire activated!\nTask done.");
                matchesIndicator.SetActive(false);
                TaskStatus.OnValueChanged -= OnTaskStatusChange;
                break;
        }
    }

    // Have we gathered enough twigs and the logs are present?
    private bool LogStackMade() {
        // return TaskStatus.Value >= 1;
        return TaskStatus.Value >= FireTaskStatus.TwigsDone;
    }

    // Have we lit the fire?
    private bool FireMade() {
        return TaskStatus.Value >= FireTaskStatus.Done;
    }

    // Handle when twigs or matches get dropped in pit
    private void OnTriggerEnter(Collider other)
    {
        int instance_id = other.gameObject.GetInstanceID();
        // Don't execute if we're a client,
        // only server manages state. Also
        // if fire is already made we dont care.
        // Oh, also if we've already seen this object
        // and it''s triggering a duplicate collision.
        if (IsClient || FireMade() || objects_inside.Contains(instance_id)) {
            return;
        }

        // If we got firewood and logs aren't already made
        if (other.CompareTag("Firewood") && !LogStackMade())
        {
            twigsCount.Value++;
            objects_inside.Add(instance_id);
        }
        // Otherwise if we got a match and 
        else if (other.CompareTag("Match") && LogStackMade() )
        {
            matchCount.Value++;
            objects_inside.Add(instance_id);
        }

    }

    // Handle when twigs leave pit
    private void OnTriggerExit(Collider other)
    {
        // Don't execute if we're a client,
        // only server manages state
        if (IsClient || LogStackMade()) {
            return;
        }

        int instance_id = other.gameObject.GetInstanceID();

        if (other.CompareTag("Firewood"))
        {
            twigsCount.Value--;
            objects_inside.Remove(instance_id);
        }
        else if (other.CompareTag("Match") && LogStackMade() )
        {
            matchCount.Value--;
            objects_inside.Remove(instance_id);
        }
    }

    // Construct the logs, destroying any twigs inside the collider
    private void MakeLogs() {
        // Set the campfire prefab active
        logstack.SetActive(true);

        // Server despawns all firewood objects in the trigger zone
        if (IsClient) {
            return;
        }

        // Clean up twigs
        DespawnItemsInside("Firewood");
    }

    // Construct the fire, destroying any matches inside the collider
    private void MakeFire()
    {
        fireIndicator.SetActive(false);
        fire.SetActive(true);
        fireLight.SetActive(true);
        // fireMade = true;

        if (IsClient) {
            return;
        }

        // Clean up matches
        DespawnItemsInside("Match");
    }

    // Despawns/destroys items inside collider with given tag.
    // Server executed.
    private void DespawnItemsInside(string tag) {
        if (IsClient) {
            return;
        }

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                GameObject go = collider.gameObject;
                NetworkObject no = go.GetComponent<NetworkObject>();
                if (no != null) {
                    if (!no.IsOwnedByServer) {
                        no.RemoveOwnership();
                    }
                    no.Despawn();
                    // Debug.Log($"Firepit: Despawned object with tag {tag}");
                }
                else {
                    Destroy(gameObject);
                    // Debug.Log($"Firepit: Destroyed object with tag {tag}");
                }
            }
        }
    }
}
