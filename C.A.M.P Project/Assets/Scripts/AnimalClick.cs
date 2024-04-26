using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalClick : MonoBehaviour
{
    public void OnDeselectGrabbable(SelectExitEventArgs eventArgs) {
        GameObject selected = eventArgs.interactableObject.transform.gameObject;
        if (selected.layer == LayerMask.NameToLayer("Animals")) {
            HandleReleaseAnimal(selected);
            return;
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs) {
        GameObject selected = eventArgs.interactableObject.transform.gameObject;

        if (selected.layer == LayerMask.NameToLayer("Animals")) {
            HandleGrabAnimal(selected);
            return;
        }
    }

    private void HandleGrabAnimal(GameObject animal) {
        AnimalWiki wiki = animal.GetComponent<AnimalWiki>();
        if (wiki == null) {
            Debug.Log("Tried to get wiki on animal but it was null");
        }
        Debug.Log($"Grabbed Animal: {wiki.GetAnimalDesc()}");
        // TODO
        // get animal text
        // set animal text
        // show display
    }

    private void HandleReleaseAnimal(GameObject animal) {
        // TODO: if deselecting animal, hide animal text pop up after delay
        Debug.Log("Released animal");

    }
}
