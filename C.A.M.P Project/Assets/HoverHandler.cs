using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverHandler : MonoBehaviour
{
    // public ObjectInfoDisplay infoDisplay;

    private void Start()
    {
        var interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
        interactable.selectEntered.AddListener(OnSelectEnter);
        interactable.selectExited.AddListener(OnSelectExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        GameObject hoveredObject = args.interactable.gameObject;
        Debug.Log("OnHover" + args.interactable.gameObject);
        // infoDisplay.DisplayObjectInfo(hoveredObject);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        // infoDisplay.HideObjectInfo();
        Debug.Log("ExitHover" + args.interactable.gameObject);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        GameObject selectedObject = args.interactable.gameObject;
        Debug.Log("OnSelect" + args.interactable.gameObject);
        // infoDisplay.DisplayObjectInfo(selectedObject);
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        // infoDisplay.HideObjectInfo();
        Debug.Log("ExitSelect" + args.interactable.gameObject);
    }
}