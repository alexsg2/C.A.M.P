
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
    // poles, 0 == left, 1 == right
    public GameObject[] pole_triggers = new GameObject[2];
    public GameObject[] pole_objects = new GameObject[2];
    public GameObject[] pole_indicators = new GameObject[2];


    // tarps, 0 == left, 1 == right
    public GameObject[] tarp_triggers = new GameObject[2];
    public GameObject[] tarp_objects = new GameObject[2]; // to enable as task is completed
    public GameObject[] tarp_indicators = new GameObject[4];


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

        taskStatus.OnValueChanged += OnTaskStatusChange;
        poles.OnValueChanged += OnPolesUpdate;
        // TODO: disable all triggers by default, need triggers to be enabled
        for (int i = 0; i < tarp_triggers.Length; i++) {
            tarp_triggers[i].SetActive(false);
        }
    }

    public void InitTask() {
        // TODO: enable poles triggers, indicators, sub to event
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
        if (updated.left && updated.right && IsServer) {
            Debug.Log("TentTaskStatus updated");
            taskStatus.Value = TentTaskStatus.PolesDone;
        }

    }

    public void OnTarpsUpdate(TwoBools old, TwoBools updated) {
        Debug.Log("Tarps have been updated");
        if (!tarp_objects[0].activeSelf && updated.left) {
            Debug.Log("TENT: activated left tarp");
            tarp_objects[0].SetActive(true);
            // TODO: disable indicators
        }

        if (!tarp_objects[1].activeSelf && updated.right) {
            Debug.Log("TENT: activated right tarp");
            tarp_objects[1].SetActive(true);
            // TODO: disable indicators
        }
        if (updated.left && updated.right && IsServer) {
            taskStatus.Value = TentTaskStatus.TarpsDone;
        }
    }

    public void OnNailsUpdate(FourBools old, FourBools updated) {
        // TODO: enable nail objs

        // if (updated.one && updated.two && updated.three && updated.four && IsServer) {
        //     // TODO: if all 4 are true, then unsubscribe from event,
        //     // update task prog
        // }
    }

    // Unsubscribe/subscribe to subtask vars,
    // set up task triggers + indicators. Previous task
    // triggers disable themselves automatically.
    public void OnTaskStatusChange(TentTaskStatus old, TentTaskStatus updated) {
        // client & server execution
        switch (updated) {
            case TentTaskStatus.Start:
                break;
            case TentTaskStatus.PolesDone:
                poles.OnValueChanged -= OnPolesUpdate;
                tarps.OnValueChanged += OnTarpsUpdate;
                // enable tarp triggers + indicators
                for (int i = 0; i < tarp_triggers.Length; i++) {
                    tarp_triggers[i].SetActive(true);
                }
                foreach (GameObject indicator in tarp_indicators) {
                    indicator.SetActive(true);
                }
                Debug.Log("Pole task done, moving on to tarps");
                break;
            case TentTaskStatus.TarpsDone:
                tarps.OnValueChanged -= OnTarpsUpdate;
                nails.OnValueChanged += OnNailsUpdate;
                // TODO: enable nails triggers
                break;
            case TentTaskStatus.Done:
                nails.OnValueChanged -= OnNailsUpdate;
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
