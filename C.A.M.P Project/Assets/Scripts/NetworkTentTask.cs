
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
    public GameObject tent_indicator;
    public GameObject tent;
    public GameObject ratchet_tent; // tent that players set up, parent
    // of all the objects we enable partway through the task

    // poles, 0 == left, 1 == right
    public GameObject[] pole_triggers = new GameObject[2];
    public GameObject[] pole_objects = new GameObject[2];
    public GameObject[] pole_placement_indicators = new GameObject[2];
    public GameObject poles_indicator;


    // tarps, 0 == left, 1 == right
    public GameObject[] tarp_triggers = new GameObject[2];
    public GameObject[] tarp_objects = new GameObject[2]; // to enable as task is completed
    public GameObject[] tarp_placement_indicators = new GameObject[2];
    public GameObject[] tarps_indicators = new GameObject[2];

    // nails, 0, 1, 2, 3
    public GameObject[] nail_placement_indicators = new GameObject[4];
    public GameObject[] nail_triggers = new GameObject[4];
    public GameObject nails_indicator;


    // default is left pole ([0]) = false, right pole ([1])= false. 
    // both are true when both poles have been installed.
    public NetworkVariable<TwoBools> poles = new NetworkVariable<TwoBools>(
        default,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    // both are true when both tarps have been installed.
    public NetworkVariable<TwoBools> tarps = new NetworkVariable<TwoBools>(
        default,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);


    // default is false, false, false, false
    // all are true when all nails have been installed fully
    public NetworkVariable<FourBools> nails = new NetworkVariable<FourBools>(
        default,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    /// <summary>
    /// Global status of this task.
    /// </summary>
    public NetworkVariable<TentTaskStatus> taskStatus = new NetworkVariable<TentTaskStatus>(
        TentTaskStatus.Wait, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
        

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        taskStatus.OnValueChanged += OnTaskStatusChange;
    }

    public void OnPolesUpdate(TwoBools old, TwoBools updated) {
        // activate the actual poles: client & server
        if (!pole_objects[0].activeSelf && updated.left) {
            // Debug.Log("TENT: activated left pole");
            pole_objects[0].SetActive(true);
            pole_placement_indicators[0].SetActive(false);
        }

        if (!pole_objects[1].activeSelf && updated.right) {
            // Debug.Log("TENT: activated right pole");
            pole_objects[1].SetActive(true);
            pole_placement_indicators[1].SetActive(false);
        }

        // Check if complete
        if (updated.left && updated.right && IsServer) {
            // Debug.Log("TentTaskStatus updated");
            taskStatus.Value = TentTaskStatus.PolesDone;
        }
    }

    public void OnTarpsUpdate(TwoBools old, TwoBools updated) {
        // Debug.Log("Tarps have been updated");
        if (!tarp_objects[0].activeSelf && updated.left) {
            // Debug.Log("TENT: activated left tarp");
            tarp_objects[0].SetActive(true);
            // TODO: disable indicators
            tarp_placement_indicators[0].SetActive(false);
        }

        if (!tarp_objects[1].activeSelf && updated.right) {
            // Debug.Log("TENT: activated right tarp");
            tarp_objects[1].SetActive(true);
            tarp_placement_indicators[1].SetActive(false);
            // TODO: disable indicators, handle in tarp thing itself?
        }

        if (updated.left && updated.right && IsServer) {
            taskStatus.Value = TentTaskStatus.TarpsDone;
        }
    }

    public void OnNailsUpdate(FourBools old, FourBools updated) {
        if (IsClient) {
            return;
        }

        // check if all 4 nails are in, then move on
        // nail triggers handle enabling nails
        if (updated.one && updated.two && updated.three && updated.four) {
            // update task status
            taskStatus.Value = TentTaskStatus.Done;
        }
    }

    // Unsubscribe/subscribe to subtask vars,
    // set up task triggers + indicators. Previous task
    // triggers disable themselves automatically.
    public void OnTaskStatusChange(TentTaskStatus old, TentTaskStatus updated) {
        // client & server execution
        switch (updated) {
            case TentTaskStatus.Start:
                StartTask();
                break;
            case TentTaskStatus.PolesDone:
                StartTarps();
                break;
            case TentTaskStatus.TarpsDone:
                StartNails();
                break;
            case TentTaskStatus.Done:
                FinishTask();
                break;
        }
    }

    private void EnablePoleTriggersAndIndicators() {
        foreach (GameObject indicator in pole_placement_indicators) {
            indicator.SetActive(true);
        }
        poles_indicator.SetActive(true);

        if (IsClient) {
            return;
        }

        foreach(GameObject trigger in pole_triggers) {
            trigger.SetActive(true);
        }
    }

    private void EnableTarpTriggersAndIndicators() {
        foreach (GameObject indicator in tarp_placement_indicators) {
            indicator.SetActive(true);
        }
        foreach (GameObject indicator in tarps_indicators) {
            indicator.SetActive(true);
        }

        if (IsClient) {
            return;
        }

        foreach (GameObject trigger in tarp_triggers) {
            trigger.SetActive(true);
        }
    }

    private void EnableNailTriggersAndIndicators() {
        // enable indicators
        foreach (GameObject indicator in nail_placement_indicators) {
            indicator.SetActive(true);
        }
        nails_indicator.SetActive(true);

        if (IsClient) {
            return;
            // CLIENTS DO NOT USE TRIGGERSS!
        }
        // enable triggers
        foreach (GameObject trigger in nail_triggers) {
            trigger.SetActive(true);    
        }
    }

    public void StartTask() {
        // Debug.Log("Start task called");
        // start first subtask: poles
        tent_indicator.SetActive(true);
        poles.OnValueChanged += OnPolesUpdate;
        EnablePoleTriggersAndIndicators(); 
    }

    private void StartTarps() {
        // move to second subtask: tarps
        poles_indicator.SetActive(false);
        poles.OnValueChanged -= OnPolesUpdate;
        tarps.OnValueChanged += OnTarpsUpdate;
        EnableTarpTriggersAndIndicators();
        // Debug.Log("Pole task done, moving on to tarps");
    }

    private void StartNails() {
        // move to final subtask: nails
        foreach (GameObject indicator in tarps_indicators) {
            indicator.SetActive(false);
        }
        tarps.OnValueChanged -= OnTarpsUpdate;
        nails.OnValueChanged += OnNailsUpdate;
        EnableNailTriggersAndIndicators();
        // Debug.Log("Tarp task done, moving on to nails");
    }

    private void FinishTask() {
        // wrap up the task
        nails_indicator.SetActive(false);
        tent_indicator.SetActive(false);
        ratchet_tent.SetActive(false);
        tent.SetActive(true);
        // triggers disable their indicators and selves
        nails.OnValueChanged -= OnNailsUpdate;
        taskStatus.OnValueChanged -= OnTaskStatusChange;
        // Debug.Log("Tent task complete!");
    }

    /// <summary>
    /// Enable certain parts based on task status.
    /// </summary>
    public void FastForward() {
        // TODO: for players that rejoin/join late
        // handle enabling triggers as well if necessary
        // Debug.Log("TODO: tent task client fast forward");
        // erm, do we have to fast forward each subtask then? what if one subtask is partially complete?
        // then when it completes things will update, not too terrible
        if (taskStatus.Value == TentTaskStatus.Done) {

        }
        else {
            taskStatus.OnValueChanged += OnTaskStatusChange; // listen for task changes brother
            // move this elsewhere? confusing
        }
    }
}
