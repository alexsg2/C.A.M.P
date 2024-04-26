
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
    // public GameObject[] pole_triggers = new GameObject[2];

    public enum TentTaskStatus {
        Start,
        PolesDone,
        TarpsDone,
        Done // after nails are done
    }

    public enum Side {
        left,
        right
    }

    public enum Count {
        one,
        two,
        three,
        four
    }

    public struct TwoBools : INetworkSerializable
    {
        public bool left;
        public bool right;

        public TwoBools(bool l, bool r) {
            left = l;
            right = r;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref left);
            serializer.SerializeValue(ref right);
        }
    }

    public struct FourBools : INetworkSerializable
    {
        public bool one;
        public bool two;
        public bool three;
        public bool four;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref one);
            serializer.SerializeValue(ref two);
            serializer.SerializeValue(ref three);
            serializer.SerializeValue(ref three);
        }
    }

    // default is left = false, right = false
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
    public NetworkVariable<TentTaskStatus> TaskStatus = new NetworkVariable<TentTaskStatus>(
        TentTaskStatus.Start, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
        

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        poles.OnValueChanged += OnPolesUpdate;
    }

    public void InitTask() {
        // TODO: enable poles triggers
        // enable highlights n shit
    }

    public void OnPolesUpdate(TwoBools old, TwoBools updated) {

        if (updated.left && updated.right) {
            // TODO: if both are true, then unsubscribe from event
            // enable tarps triggers, subscribe to their event, update task prog
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

    // public void OnTaskStatusChange(TentTaskStatus old, TentTaskStatus updated) {
    //     // TODO: handle enabling things / disabling things, moving between subtasks
    //     // disable listeners for early subtasks

    //     // We don't need this if we're updating through each subtasks network thingy
    // }
    public void FastForward() {
        // TODO: for players that rejoin/join late
    }
}
