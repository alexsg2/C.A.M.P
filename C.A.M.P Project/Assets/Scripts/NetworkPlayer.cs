using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class NetworkPlayer : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DisableClientInput();
    }

    /// <summary>
    /// Disable this client's input affecting this player object if they do not own it.
    /// </summary>
    public void DisableClientInput() {
        if (!IsOwner) {
            NetworkMoveProvider clientMoveProvider = GetComponentInChildren<NetworkMoveProvider>();
            ActionBasedController[] clientControllers = GetComponentsInChildren<ActionBasedController>();
            ActionBasedContinuousTurnProvider clientTurnProvider = GetComponentInChildren<ActionBasedContinuousTurnProvider>();
            TrackedPoseDriver clientHead = GetComponentInChildren<TrackedPoseDriver>();
            Camera clientCamera = GetComponentInChildren<Camera>();
            AudioListener clientAudioListener = GetComponent<AudioListener>();

            clientCamera.enabled = false;
            clientMoveProvider.enableInputActions = false;
            // clientTurnProvider.enableTurnLeftRight = false;
            // clientTurnProvider.enableTurnAround = false;
            clientTurnProvider.enabled = false;
            clientHead.enabled = false;
            clientAudioListener.enabled = false;

            foreach (var controller in clientControllers) {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
                controller.GetComponent<XRInteractorLineVisual>().enabled = false;
            }
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs) {
        if (IsClient && IsOwner) {
            Debug.Log("Trying to change ownership");
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            if (networkObjectSelected != null) {
                // request ownership of interactable from the server
                // TODO: make server spawn grabbables or be the only one that has networked grabbables in scene
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
                // RPC = remote procedure call, call procedure on network object not in this executable/client
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference) {
        if (networkObjectReference.TryGet(out NetworkObject networkObject)) {
            networkObject.ChangeOwnership(newOwnerClientId);
        }
        else {
            Debug.Log($"Unable to change ownership for clientId {newOwnerClientId}");
        }
    }
}
