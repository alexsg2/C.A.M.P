
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

// Network Behavior for tent task. Handles variables for
// task like pole/tarp/nail statuses, as well as behavior
// like actually activating nails/tarps/poles.
public class NetworkTentTask : NetworkBehaviour
{
    // 0 == left, 1 == right
    public GameObject[] pole_objects = new GameObject[2];
    public GameObject[] pole_indicators = new GameObject[2];


    // default is left pole ([0]) = false, right pole ([1])= false. 
    public NetworkVariable<TwoBools> poles = new NetworkVariable<TwoBools>(
        default,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    public NetworkVariable<TwoBools> tarps = new NetworkVariable<TwoBools>(
        default,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<FourBools> nails = new NetworkVariable<FourBools>(
        default,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    /// <summary>
    /// Global status of this task.
    /// </summary>
    public NetworkVariable<TentTaskStatus> taskStatus = new NetworkVariable<TentTaskStatus>(
        TentTaskStatus.Start, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
        

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        poles.OnValueChanged += OnPolesUpdate;
    }

    public void InitTask() {
        // TODO: enable poles triggers, indicators
        // enable highlights n shit. all tent task triggers/stuff should be disabled by default.
    }

    public void OnPolesUpdate(TwoBools old, TwoBools updated) {
        // activate the actual poles: client & server
        if (!pole_objects[0].activeSelf && updated.left) {
            Debug.Log("TENT: activated left pole");
            pole_objects[0].SetActive(true);
        }

        if (!pole_objects[1].activeSelf && updated.right) {
            Debug.Log("TENT: activated right pole");
            pole_objects[1].SetActive(true);
        }

        // Check if complete
        if (updated.left && updated.right) {
            // update task status
            if (IsServer) {
                taskStatus.Value = TentTaskStatus.PolesDone;
            }
        }

    }

    public void OnTarpsUpdate(TwoBools old, TwoBools updated) {
        if (updated.left && updated.right) {
            // TODO: if both are true, then unsubscribe from event,
            // enable nails triggers, subscribe to their event, update task prog
        }

    }

    public void OnNailsUpdate(FourBools old, FourBools updated) {
        if (updated.one && updated.two && updated.three && updated.four) {
            // TODO: if all 4 are true, then unsubscribe from event,
            // update task prog
        }
    }

    public void OnTaskStatusChange(TentTaskStatus old, TentTaskStatus updated) {
        switch (updated) {
            case TentTaskStatus.Start:
                break;
            case TentTaskStatus.PolesDone:
                poles.OnValueChanged -= OnPolesUpdate;
                // TODO: enable tarps triggers, subscribe to their event
                break;
            case TentTaskStatus.TarpsDone:
                // TODO: 
                break;
            case TentTaskStatus.Done:
                break;
        }
    }

    // public void OnTaskStatusChange(TentTaskStatus old, TentTaskStatus updated) {
    //     // TODO: handle enabling things / disabling things, moving between subtasks
    //     // disable listeners for early subtasks

    //     // We don't need this if we're updating through each subtasks network thingy
    // }
    public void FastForward() {
        // TODO: for players that rejoin/join late
    }
}
