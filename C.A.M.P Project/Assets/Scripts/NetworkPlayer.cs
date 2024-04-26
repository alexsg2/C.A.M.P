using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class NetworkPlayer : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DisableClientInput();

        // NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
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
            AudioListener clientAudioListener = GetComponentInChildren<AudioListener>();

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
        else {
            MeshRenderer[] models = GetComponentsInChildren<MeshRenderer>();

            // Disable player model for self so can see through cam

            foreach (MeshRenderer mesh in models) {
                if (mesh.gameObject.CompareTag("PLayer Model")) {
                    mesh.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs) {
        // Debug.Log("OnSelectGrabbable");
        if (IsClient && IsOwner) {
            // Debug.Log("OnSelectGrabbable inside first if");
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            if (networkObjectSelected.gameObject.layer != LayerMask.NameToLayer("Grabbable")) {
                return;
            }
            if (networkObjectSelected != null && networkObjectSelected.OwnerClientId != OwnerClientId) {
                // Debug.Log("OnSelectGrabbable inside second if");
                // request ownership of interactable from the server
                // TODO: handle race conditions for trying to change ownership of an object
                Debug.Log("Requesting ownership of grabbable");
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
                // RPC = remote procedure call, call procedure on network object not in this executable/client
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId,
        NetworkObjectReference networkObjectReference) 
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject)) {
            networkObject.ChangeOwnership(newOwnerClientId);
            Debug.Log($"Updated ownership to clientId {newOwnerClientId}");
        }
        else {
            Debug.Log($"Unable to change ownership for clientId {newOwnerClientId}");
        }
    }
}
