using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{

    public GameObject animalTextDisplay;
    public Text animalText;

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
                if (mesh.gameObject.CompareTag("Player Model")) {
                    mesh.enabled = false; // just disable renderer
                }
            }
        }
    }

    private void HandleGrabAnimal(GameObject animal) {
        AnimalWiki wiki = animal.GetComponent<AnimalWiki>();
        if (wiki == null) {
            Debug.Log("Tried to get wiki on animal but it was null");
        }
        animalText.text = wiki.GetAnimalDesc();
        animalTextDisplay.SetActive(true);
        // Debug.Log($"Grabbed Animal: {wiki.GetAnimalDesc()}");
        // TODO
        // get animal text
        // set animal text
        // show display
    }

    private void HandleReleaseAnimal(GameObject animal) {
        // TODO: if deselecting animal, hide animal text pop up after delay
        StartCoroutine(DelayedDisableAnimalText());
    }

    //Removes text after a certain period of time
    private IEnumerator DelayedDisableAnimalText()
    {
        yield return new WaitForSeconds(5);
        animalText.text = "";
        animalTextDisplay.SetActive(false);
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs) {
        // Debug.Log("OnSelectGrabbable");
        if (IsClient && IsOwner) {
            // TODO: check if layer is animal, get animal description thingy, get text, set text on player animal text, pop it up
            // Debug.Log("OnSelectGrabbable inside first if");

            GameObject selected = eventArgs.interactableObject.transform.gameObject;

            if (selected.layer == LayerMask.NameToLayer("Animals")) {
                HandleGrabAnimal(selected);
                return;
            }

            NetworkObject networkObjectSelected = selected.GetComponent<NetworkObject>();

            if (networkObjectSelected == null) {
                return;
            }

            // handle grabbables, request ownership
            if (networkObjectSelected.gameObject.layer == LayerMask.NameToLayer("Grabbable") && networkObjectSelected.OwnerClientId != OwnerClientId) {
                // TODO: handle race conditions
                // Debug.Log("Requesting ownership of grabbable");
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
            }
         }
    }

    public void OnDeselectGrabbable(SelectExitEventArgs eventArgs) {
        if (IsClient && IsOwner) {
            GameObject selected = eventArgs.interactableObject.transform.gameObject;
            if (selected.layer == LayerMask.NameToLayer("Animals")) {
                HandleReleaseAnimal(selected);
                return;
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId,
        NetworkObjectReference networkObjectReference) 
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject)) {
            networkObject.ChangeOwnership(newOwnerClientId);
            // Debug.Log($"Updated ownership to clientId {newOwnerClientId}");
        }
        else {
            // Debug.Log($"Unable to change ownership for clientId {newOwnerClientId}");
        }
    }
}
