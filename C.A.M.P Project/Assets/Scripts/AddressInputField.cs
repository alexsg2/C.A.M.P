using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddressInputField : MonoBehaviour
{
    /// <summary>
    /// Called when we select the input field.
    /// </summary>
    public void OnSelect() {
        TouchScreenKeyboard.hideInput = false;
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        Debug.Log("Selected address field");
    }

    // NOTE: need invisible putton whose onclick() calls InputField's Select() method! select() is weird in quest

    public void OnDeselect() {
        Debug.Log("Deselected address field");
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

    public void OnClickButton() {
        Debug.Log("Clicked button");
    }
}
