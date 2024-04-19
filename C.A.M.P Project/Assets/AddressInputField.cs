using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddressInputField : MonoBehaviour
{

    public GameObject thing;
    /// <summary>
    /// Called when we select the input field.
    /// </summary>
    public void OnSelect() {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        
        thing.SetActive(!thing.activeSelf);
    }

    public void OnDeselect() {
        Debug.Log("Deselected address field");
        thing.SetActive(!thing.activeSelf);
    }

    /// <summary>
    /// Called when we deselect/stop editing field.
    /// </summary>
    public void OnEndEdit() {
        Debug.Log("Stopped editing address field");
    }

    /// <summary>
    /// Called when we type something into input field.
    /// </summary>
    public void OnValueChanged() {
        Debug.Log("Address field value changed");
    }
}
