using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

// Network Behavior for fire task. Handles variables for
// task like twig count and match count, as well as behavior
// like actually activating logs / fire.
public class NetworkFireTask : NetworkBehaviour
{
    // Objects to toggle as we go!
    public GameObject logstack; // Logs to toggle visibility
    public GameObject fire;
    public GameObject fireLight;

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
    public NetworkVariable<int> TaskStatus = new NetworkVariable<int>(
        0,  
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server); 

    [SerializeField]
    private int twigsRequired = 12; // Number of twigs required

    [SerializeField]
    private int matchesRequired = 3; // Number of matches required

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient && TaskStatus.Value != 0) {
            // let joining client catch up
            FastForward();
        }
        else {
            twigsCount.OnValueChanged += OnTwigCountChange;
            TaskStatus.OnValueChanged += OnTaskStatusChange;
        }
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
            // if fire is not made, add listener
            matchCount.OnValueChanged += OnMatchCountChange;
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
        Debug.Log("Twigs count: " + updated);

        if (IsServer) {
            if (updated >= twigsRequired) {
                TaskStatus.Value = 1;
            }
        }
    }

    // Debug log when match count updates,
    // server checks if we met necessary match amount
    // and updated Task1Status accordingly.
    public void OnMatchCountChange(int old, int updated) {
        Debug.Log("Matches count: " + matchCount.Value);

        if (IsServer) {
            if (updated >= matchesRequired) {
                TaskStatus.Value = 2;
            }
        }
    }

    // Handle fire state when our task completion state
    // changes.
    public void OnTaskStatusChange(int old, int updated) {
        // both servers and clients execute this
        switch (updated) {
            case 0:
                Debug.Log("Start firepit task. GET SOME TWIGS!");
                break;
            case 1:
                MakeLogs();
                Debug.Log("Enough twigs have been gathered, logs activated!");
                twigsCount.OnValueChanged -= OnTwigCountChange;
                matchCount.OnValueChanged += OnMatchCountChange;
                break;
            case 2:
                // enable fire
                // deregister listener for match count
                MakeFire();
                Debug.Log("Enough lit matches have been gathered, fire activated!\nTask done.");
                TaskStatus.OnValueChanged -= OnTaskStatusChange;
                break;
        }
    }

    // Have we gathered enough twigs and the logs are present?
    private bool LogStackMade() {
        return TaskStatus.Value >= 1;
    }

    // Have we lit the fire?
    private bool FireMade() {
        return TaskStatus.Value >= 2;
    }

    // Handle when twigs or matches get dropped in pit
    private void OnTriggerEnter(Collider other)
    {
        // Don't execute if we're a client,
        // only server manages state. Also
        // if fire is already made we dont care.
        if (IsClient || FireMade()) {
            return;
        }

        // If we got firewood and logs aren't already made
        if (other.CompareTag("Firewood") && !LogStackMade())
        {
            twigsCount.Value++;
        }
        // Otherwise if we got a match and 
        else if (other.CompareTag("Match") && LogStackMade() )
        {
            matchCount.Value++;
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

        if (other.CompareTag("Firewood"))
        {
            twigsCount.Value--;
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
                    Debug.Log($"Firepit: Despawned object with tag {tag}");
                }
                else {
                    Destroy(gameObject);
                    Debug.Log($"Firepit: Destroyed object with tag {tag}");
                }
            }
        }
    }
}
