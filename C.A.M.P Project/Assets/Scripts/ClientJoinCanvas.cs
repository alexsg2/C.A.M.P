using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem.Controls;
using System;

public class ClientJoinCanvas : MonoBehaviour
{
    public Button joinButton;
    public Button cancelButton;

    public TMP_InputField addressField;
    public TMP_InputField portField;

    // public TextMeshProUGUI portDisplay;

    private Regex ipPattern;
    private Regex portPattern;
    private string ip;
    private ushort port;

    private UnityTransport tp;

    public void Start() {
        joinButton.interactable = true;
        cancelButton.interactable = false;

        tp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        ip = tp.ConnectionData.Address;
        port = tp.ConnectionData.Port; // use port on network manager
        // portDisplay.text = $"Port: {port}";
        portField.text = ((int) port).ToString();
        addressField.text = ip;

        ipPattern = new Regex(@"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$");
        portPattern = new Regex(@"^(0|[1-9][0-9]*)$");
    }

    // Join server
    public void JoinServer()
    {
        NetworkManager.Singleton.StartClient();
        // Debug.Log("Joining server");
        joinButton.interactable = false;
        cancelButton.interactable = true;
    }

    // Cancel joining a server
    public void Cancel() {
        NetworkManager.Singleton.Shutdown();
        // Debug.Log("Canceled Join");
        joinButton.interactable = true;
        cancelButton.interactable = false;
    }

    /// <summary>
    /// Called onStoppedEditing for address input field.
    /// Validates ip address and changes text value and
    /// connection settings if valid.
    /// </summary>
    public void ValidateAddress() {
        string potential = addressField.text;
        // Debug.Log(potential);
        if (ipPattern.IsMatch(potential)) {
            ip = potential;
            tp.SetConnectionData(ip, port);
            // Debug.Log("Address accepted");
        }
        else {
            // Reset field
            addressField.text = ip;
            // Debug.Log("Address rejected");
        }
    }

    /// <summary>
    /// Called onStoppedEditing for address input field.
    /// Validates port and changes text value and
    /// connection settings if valid.
    /// </summary>
    public void ValidatePort() {
        string potential = portField.text;
        if (portPattern.IsMatch(potential)) {
            port = (ushort) Int16.Parse(potential);
            tp.SetConnectionData(ip, port);
            // Debug.Log($"Port # accepted and updated to {port}");
        }
        else {
            portField.text = ((int) port).ToString();;
            // Debug.Log("Port # rejected");
        }
    }

    // Called OnSelect() from address & port input field.
    public void OpenKeyboard() {
        TouchScreenKeyboard.hideInput = false;
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        // Debug.Log("Selected address field");
    }
}
